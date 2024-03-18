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
    public interface IQuizesCreator
    {
        Task<Quiz?> CreateAsync(
		   CreateQuizInput input
			);
        Task<IEnumerable<Question>?> GetQuizQuestionsAsync(
            string technologyName,
            AdvanceNumber advanceNumber,
            Guid quizId);
	}
}
