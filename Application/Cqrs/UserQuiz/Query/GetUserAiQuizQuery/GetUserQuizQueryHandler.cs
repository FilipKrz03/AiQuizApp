using Application.Dto;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Cqrs.UserQuiz.Query.GetUserAiQuizQuery
{
	public sealed class GetUserQuizQueryHandler(
		IRepository<Domain.Entities.Quiz> quizRepository,
		IUserRepository userRepository,
		IMapper mapper
		) : IRequestHandler<GetUserQuizQuery, QuizDetailResponseDto>
	{
		private readonly IRepository<Domain.Entities.Quiz> _quizRepository = quizRepository;
		private readonly IUserRepository _userRepository = userRepository;
		private readonly IMapper _mapper = mapper;

		public async Task<QuizDetailResponseDto> Handle(GetUserQuizQuery request, CancellationToken cancellationToken)
		{
			if (!await _userRepository.UserExistAsync(request.UserId))
			{
				throw new InvalidTokenClaimException();
			}

			var userQuiz = await _quizRepository
				.GetByIdQuery(request.QuizId)
				.Where(x => x.UserId == request.UserId)
				.Include(x => x.Questions)
				.ThenInclude(x => x.Answers)
				.FirstOrDefaultAsync(cancellationToken);

			if (userQuiz == null)
			{
				throw new ResourceNotFoundException(request.QuizId, "User quiz");
			}

			return _mapper.Map<QuizDetailResponseDto>(userQuiz)!;
		}
	}
}
