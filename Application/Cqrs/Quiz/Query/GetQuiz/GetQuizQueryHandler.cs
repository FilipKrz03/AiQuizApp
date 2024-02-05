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

namespace Application.Cqrs.Quiz.Query.GetQuiz
{
    public class GetQuizQueryHandler : IRequestHandler<GetQuizQuery, QuizDetailResponseDto>
    {
        private readonly IRepository<Domain.Entities.Quiz> _quizRepository;
        private readonly IMapper _mapper;

        public GetQuizQueryHandler(
            IRepository<Domain.Entities.Quiz> quizRepository,
            IMapper mapper
            )
        {
            _quizRepository = quizRepository;
            _mapper = mapper;
        }

        public async Task<QuizDetailResponseDto> Handle(GetQuizQuery request, CancellationToken cancellationToken)
        {
            var quiz = await _quizRepository
                .GetByIdQuery(request.Id)
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync();

            if (quiz == null) throw new ResourceNotFoundException(request.Id, "Quiz");
           
            return _mapper.Map<QuizDetailResponseDto>(quiz)!;
        }
    }
}
