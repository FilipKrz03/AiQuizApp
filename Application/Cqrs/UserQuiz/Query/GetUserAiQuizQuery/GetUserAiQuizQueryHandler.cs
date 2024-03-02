using Application.Dto;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Cqrs.UserQuiz.Query.GetUserAiQuizQuery
{
	public sealed class GetUserAiQuizQueryHandler(
		IRepository<UserOwnQuiz> userOwnQuizRepository,
		IUserRepository userRepository,
		IMapper mapper
		) : IRequestHandler<GetUserAiQuizQuery, QuizDetailResponseDto>
	{
		private readonly IRepository<UserOwnQuiz> _userOwnQuizRepository = userOwnQuizRepository;
		private readonly IUserRepository _userRepository = userRepository;
		private readonly IMapper _mapper = mapper;

		public async Task<QuizDetailResponseDto> Handle(GetUserAiQuizQuery request, CancellationToken cancellationToken)
		{
			if (!await _userRepository.UserExistAsync(request.UserId))
			{
				throw new InvalidTokenClaimException();
			}

			var userQuiz = await _userOwnQuizRepository
				.GetByIdQuery(request.QuizId)
				.Where(x => x.UserId == request.UserId)
				.Include(x => x.Questions)
				.ThenInclude(x => x.Answers)
				.FirstOrDefaultAsync(cancellationToken);

			return _mapper.Map<QuizDetailResponseDto>(userQuiz)!;
		}
	}
}
