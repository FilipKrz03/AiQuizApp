using Application.Dto;
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
	public sealed class BaseQuizesManager(
		ILogger<BaseQuizesManager> logger,
		IServiceProvider serviceProvider
			) : BackgroundService
	{
		private readonly ILogger<BaseQuizesManager> _logger = logger;
		private readonly IServiceProvider _serviceProvider = serviceProvider;

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			using IServiceScope scope = _serviceProvider.CreateScope();
			{
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

				_logger.LogInformation(
					"BaseQuizesManager - {count} base quizes will be created",
					baseTechnologiesWithNoQuizes.Count
					);

				foreach (var technology in baseTechnologiesWithNoQuizes)
				{
					var quiz = await quizesCreator.CreateAsync(new CreateQuizInput(technology, DrawAdvanceNumber(), null));

					if (quiz != null)
					{
						quizRepository.Insert(quiz);
						await quizRepository.SaveChangesAsync();
						_logger.LogInformation("BaseQuizesManager - new base quize added to db");
					}
					else
					{
						_logger.LogWarning("Failed to create quiz : {q}" , technology);
					}
				}
				_logger.LogInformation("Base quizes manager - all base quizes added");
			}
		}

		private AdvanceNumber DrawAdvanceNumber()
		{
			Random random = new();

			return AdvanceNumber.Create(random.Next(1, 10))!;
		}
	}
}
