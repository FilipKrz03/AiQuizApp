using Application.Interfaces;
using Application.Props;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.DbContexts;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BaseQuizesManager : BackgroundService
    {
        private readonly ILogger<BaseQuizesManager> _logger;
        private readonly IServiceProvider _serviceProvider;

        public BaseQuizesManager( 
            ILogger<BaseQuizesManager> logger , 
            IServiceProvider serviceProvider
            )
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = _serviceProvider.CreateScope();
            var quizRepository = scope.ServiceProvider.GetRequiredService<IRepository<Quiz>>();
            var quizesCreator = scope.ServiceProvider.GetRequiredService<IQuizesCreator>();

            var allQuizesTechnologies =
                await quizRepository.Query()
                .Select(e => e.TechnologyName)
                .Distinct()
                .ToListAsync();

            var baseTechnologiesWithNoQuizes =
                BaseTechnologies.Get()
                .Where(t => !allQuizesTechnologies.Contains(t))
                .ToList();

            foreach(var technology in baseTechnologiesWithNoQuizes)
            {
                await quizesCreator.Create(technology , DrawAdvanceNumber() , null);
            }
        }

        private AdvanceNumber DrawAdvanceNumber()
        {
            Random random = new();

            return AdvanceNumber.Create(random.Next(0, 10))!;
        }
    }
}
