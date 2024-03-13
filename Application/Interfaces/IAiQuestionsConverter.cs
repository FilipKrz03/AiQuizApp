using Application.Dto;
using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAiQuestionsConverter
    {
        Quiz ConvertToQuiz(
            IEnumerable<QuestionAiResponseDto> response,
            string technologyName,
            AdvanceNumber advanceNumber ,
            string? quizTitle
            );
        List<Question> ConvertToQuestions(
            IEnumerable<QuestionAiResponseDto> response,
            Guid quizId
            );
	}
}
