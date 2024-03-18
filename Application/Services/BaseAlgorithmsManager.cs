using Application.Dto;
using Application.Interfaces;
using Application.Props;
using Domain.Entities;
using Domain.ValueObjects;
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
	public class BaseAlgorithmsManager(
		ILogger<BaseAlgorithmsManager> logger,
		IServiceProvider serviceProvider
		) : BackgroundService
	{
		private readonly ILogger<BaseAlgorithmsManager> _logger = logger;
		private readonly IServiceProvider _serviceProvider = serviceProvider;

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			using IServiceScope scope = _serviceProvider.CreateScope();
			{
				var algorithemsCreator = scope.ServiceProvider.GetRequiredService<IAlgorithmsCreator>();
				var algoithmTasksRepository = scope.ServiceProvider.GetRequiredService<IRepository<AlgorithmTask>>();

				var allAlgorithmsTopics =
					   await algoithmTasksRepository
					   .Query()
					   .Select(e => e.TaskMainTopics)
					   .Distinct()
					   .ToListAsync();


				var baseTopicsWithNoAlgoritmsTasks =
					BaseAlgorithmsTasksTopics.Get()
					.Where(t => !allAlgorithmsTopics.Contains(t))
					.ToList();

				foreach (var topic in baseTopicsWithNoAlgoritmsTasks)
				{
					var advanceNumber = DrawAdvanceNumber();

					var algorithm = await algorithemsCreator
						.CreateAsync(new CreateAlgorithmInput(advanceNumber, $"Task {advanceNumber.Number} / 10", topic));

					if (algorithm != null)
					{
						algoithmTasksRepository.Insert(algorithm);
						await algoithmTasksRepository.SaveChangesAsync();

						_logger.LogInformation("BaseAlgorithmsManager - new base algorithm added to db");
					}
				}
			}
		}

		private AdvanceNumber DrawAdvanceNumber()
		{
			Random random = new();

			return AdvanceNumber.Create(random.Next(1, 10))!;
		}
	}
}
