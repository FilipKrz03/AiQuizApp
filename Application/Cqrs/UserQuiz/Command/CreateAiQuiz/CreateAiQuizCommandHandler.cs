using Application.Interfaces;
using AutoMapper;
using Azure.Core;
using Domain.Entities;
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
	public sealed class CreateAiQuizCommandHandler(
		IServiceProvider serviceProvider
		)
		: IRequestHandler<CreateAiQuizCommand , string>
	{
		private readonly IServiceProvider _serviceProvider = serviceProvider;

		public Task<string> Handle(CreateAiQuizCommand request, CancellationToken cancellationToken)
		{
			CreateQuizAsync(request); // This is bacground job

			return Task.FromResult("Quiz creation queued");
		}

		private async void CreateQuizAsync(CreateAiQuizCommand request)
		{
			var scope = _serviceProvider.CreateScope();

			var quizesCreator = scope.ServiceProvider.GetRequiredService<IQuizesCreator>();
			var userOwnQuizRepository = scope.ServiceProvider.GetRequiredService<IRepository<UserOwnQuiz>>();
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<CreateAiQuizCommandHandler>>();

			var newQuiz = await quizesCreator.CreateAsync(
			request.TechnologyName,
			AdvanceNumber.Create(request.AdvanceNumber)!,
			request.QuizTitle
			);

			if (newQuiz == null)
			{
				logger.LogWarning(
					"Failed to create quiz, quizTechnology : {t} , advanceNumber : {a}",
					request.TechnologyName,
					request.AdvanceNumber
					);

				return;
			}

			UserOwnQuiz userOwnQuiz =
				new(newQuiz.Id, newQuiz.Title, newQuiz.TechnologyName, newQuiz.AdvanceNumber, request.UserId)
				{
					Questions = newQuiz.Questions
				};

			userOwnQuizRepository.Insert(userOwnQuiz);
			await userOwnQuizRepository.SaveChangesAsync();
		}
	}
}
