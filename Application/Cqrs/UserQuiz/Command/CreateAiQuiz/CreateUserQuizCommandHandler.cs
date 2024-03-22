using Application.Interfaces;
using AutoMapper;
using Azure.Core;
using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserQuiz.Command.CreateAiQuiz
{
	public sealed class CreateUserQuizCommandHandler(
		IServiceProvider serviceProvider,
		IUserRepository userRepository  
		)
		: IRequestHandler<CreateUserQuizCommand, string>
	{
		private readonly IServiceProvider _serviceProvider = serviceProvider;
		private readonly IUserRepository _userRepository = userRepository;

		public async Task<string> Handle(CreateUserQuizCommand request, CancellationToken cancellationToken)
		{
			if(!await _userRepository.UserExistAsync(request.UserId))
			{
				throw new InvalidTokenClaimException();
			}

			CreateQuizAsync(request); // This is bacground job

			return ("Quiz creation queued");
		}

		private async void CreateQuizAsync(CreateUserQuizCommand request)
		{
			var scope = _serviceProvider.CreateScope();

			var quizesCreator = scope.ServiceProvider.GetRequiredService<IQuizesCreator>();
			var userOwnQuizRepository = scope.ServiceProvider.GetRequiredService<IRepository<UserOwnQuiz>>();
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<CreateUserQuizCommandHandler>>();
			var questionRepository = scope.ServiceProvider.GetRequiredService<IRepository<Question>>();

			var quizId = Guid.NewGuid();

			UserOwnQuiz userOwnQuiz =
				new(quizId, request.QuizTitle, request.TechnologyName, AdvanceNumber.Create(request.AdvanceNumber)! , request.UserId);

			userOwnQuizRepository.Insert(userOwnQuiz);
			await userOwnQuizRepository.SaveChangesAsync();

			var quizQuestions = await quizesCreator.GetQuizQuestionsAsync(
			request.TechnologyName,
			AdvanceNumber.Create(request.AdvanceNumber)!,
			quizId
			);

			if (quizQuestions == null)
			{
				logger.LogWarning(
					"Failed to create quiz, quizTechnology : {t} , advanceNumber : {a}",
					request.TechnologyName,
					request.AdvanceNumber
					);

				userOwnQuiz.CreationStatus = CreationStatus.Failed;
				await userOwnQuizRepository.SaveChangesAsync();

				return;
			}

			userOwnQuiz.CreationStatus = CreationStatus.Succes;
			questionRepository.AddRange(quizQuestions);
			
			await userOwnQuizRepository.SaveChangesAsync();
		}
	}
}
