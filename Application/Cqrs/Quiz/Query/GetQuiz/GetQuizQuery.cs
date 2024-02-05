using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.Quiz.Query.GetQuiz
{
    public class GetQuizQuery : IRequest<QuizDetailResponseDto>
    {
        public Guid Id { get; init; }

        public GetQuizQuery(Guid id)
        {
            Id = id;
        }
    }
}
