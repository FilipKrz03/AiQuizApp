using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Enum;
using Domain.Exceptions;
using Domain.ValueObjects;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserAlgorithm.Command.CreateAlgorithm
{
	public sealed class CreateAlgorithmCommandHandler(
		IUserRepository userRepository , 
		IServiceProvider serviceProvider
		) : IRequestHandler<CreateAlgorithmCommand, string>
	{
		private readonly IUserRepository _userRepository = userRepository;
		private readonly IServiceProvider _serviceProvider = serviceProvider;

		public async Task<string> Handle(CreateAlgorithmCommand request, CancellationToken cancellationToken)
		{
			if (!await _userRepository.UserExistAsync(request.UserId))
			{
				throw new InvalidTokenClaimException();
			}

			CreateAlgorithmAsync(request); // This work in background

			return "Algorithm creation queued";
		}

		private async void CreateAlgorithmAsync(CreateAlgorithmCommand request)
		{
			var scope = _serviceProvider.CreateScope();

			var userOwnAlgorithmTaskRepository = scope.ServiceProvider.GetRequiredService<IRepository<UserOwnAlgorithmTask>>();
			var algorithmsAnswersRepository = scope.ServiceProvider.GetRequiredService<IRepository<AlgorithmAnswer>>();
			var algorithmsCreator = scope.ServiceProvider.GetRequiredService<IAlgorithmsCreator>();
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<CreateAlgorithmCommandHandler>>();

			var id = Guid.NewGuid();

			UserOwnAlgorithmTask userOwnAlgorithmTask = 
				new(id, request.TaskTitle, request.TaskMainTopics, "" , AdvanceNumber.Create(request.AdvanceNumber)! , request.UserId);

			userOwnAlgorithmTaskRepository.Insert(userOwnAlgorithmTask);
			await userOwnAlgorithmTaskRepository.SaveChangesAsync();

			var createdAlgorithm = await algorithmsCreator.CreateAlgorithmContentAndAnswersAsync(
				AdvanceNumber.Create(request.AdvanceNumber)!,
				request.TaskMainTopics,
				id
				);

			if(createdAlgorithm == null )
			{
				logger.LogWarning(
				"Failed to create algorithm, mainTopics : {mt} , advanceNumber : {a}",
				request.TaskMainTopics,
				request.AdvanceNumber
				);

				userOwnAlgorithmTask.CreationStatus = CreationStatus.Failed;
				await userOwnAlgorithmTaskRepository.SaveChangesAsync();
				return;
			}

			userOwnAlgorithmTask.TaskContent = createdAlgorithm.Value.Item1;
			algorithmsAnswersRepository.AddRange(createdAlgorithm.Value.Item2);
			userOwnAlgorithmTask.CreationStatus = CreationStatus.Succes;

			await userOwnAlgorithmTaskRepository.SaveChangesAsync();
		}
	}
}
