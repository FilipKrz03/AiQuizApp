using Application.Interfaces;
using AutoMapper;
using Azure.Core;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserQuiz.Command.CreateAiQuiz
{
	public sealed class CreateAiQuizCommandHandler(
		IRepository<UserOwnQuiz> userOwnQuizRepository,
		IQuizesCreator quizesCreator,
		ILogger<CreateAiQuizCommandHandler> logger
		)
		: IRequestHandler<CreateAiQuizCommand>
	{
		private readonly IRepository<UserOwnQuiz> _userOwnQuizRepository = userOwnQuizRepository;
		private readonly IQuizesCreator _quizesCreator = quizesCreator;
		private readonly ILogger<CreateAiQuizCommandHandler> _logger = logger;

		public async Task Handle(CreateAiQuizCommand request, CancellationToken cancellationToken)
		{
			var newQuiz = await _quizesCreator.CreateAsync(
			request.TechnologyName,
			AdvanceNumber.Create(request.AdvanceNumber)!,
			request.QuizTitle
			);

			if (newQuiz == null)
			{
				_logger.LogWarning(
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

			_userOwnQuizRepository.Insert(userOwnQuiz);
			await _userOwnQuizRepository.SaveChangesAsync();
		}
	}
}
