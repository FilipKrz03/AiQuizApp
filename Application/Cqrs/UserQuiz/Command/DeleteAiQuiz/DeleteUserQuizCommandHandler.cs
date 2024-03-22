using Application.Cqrs.UserQuiz.Query.GetUserAiQuizQuery;
using Application.Dto;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserQuiz.Command.DeleteAiQuiz
{
	public sealed class DeleteUserQuizCommandHandler(
		IRepository<UserOwnQuiz> userOwnQuizRepository,
		IUserRepository userRepository
		) : IRequestHandler<DeleteUserQuizCommand>
	{
		private readonly IRepository<UserOwnQuiz> _userOwnQuizRepository = userOwnQuizRepository;
		private readonly IUserRepository _userRepository = userRepository;

		public async Task Handle(DeleteUserQuizCommand request, CancellationToken cancellationToken)
		{
			if (!await _userRepository.UserExistAsync(request.UserId))
			{
				throw new InvalidTokenClaimException();
			}

			var quiz = await _userOwnQuizRepository
				.GetByIdQuery(request.QuizId)
				.Where(x => x.UserId == request.UserId)
				.FirstOrDefaultAsync();

			if (quiz == null)
			{
				throw new ResourceAlreadyNotExistException(request.QuizId, "User quiz");
			}

			_userOwnQuizRepository.DeleteEntity(quiz);
			await _userOwnQuizRepository.SaveChangesAsync();
		}
	}

}
