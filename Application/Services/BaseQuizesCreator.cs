using Application.Props;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BaseQuizesCreator
    {
        private readonly IRepository<Quiz> _quizRepository;

        public BaseQuizesCreator(
            IRepository<Quiz> quizRepository
            )
        {
            _quizRepository = quizRepository;            
        }

        public async Task CreateIfNotExist()
        {
            var allQuizesTechnologies =
                await _quizRepository.Query()
                .Select(e => e.TechnologyName)
                .Distinct()
                .ToListAsync();

            var baseTechnologiesWithNoQuizes =
                BaseTechnologies.Get()
                .Where(t => !allQuizesTechnologies.Contains(t))
                .ToList();

             // Todo 
        }
    }
}
