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

namespace Application.Cqrs.UserAlgorithm.Command.DeleteAlgorithm
{
	public sealed class DeleteUserAlgorithmCommandHandler(
		IRepository<AlgorithmTask> algorithmTaskRepository,
		IUserRepository userRepository
		) : IRequestHandler<DeleteUserAlgorithmCommand>
	{
		private readonly IRepository<AlgorithmTask> _algorithmTaskRepository = algorithmTaskRepository;
		private readonly IUserRepository _userRepository = userRepository;

		public async Task Handle(DeleteUserAlgorithmCommand request, CancellationToken cancellationToken)
		{
			if (!await _userRepository.UserExistAsync(request.UserId))
			{
				throw new InvalidTokenClaimException();
			}

			var algorithm = await _algorithmTaskRepository
				.GetByIdQuery(request.AlogrithmId)
				.Where(x => x.UserId == request.UserId)
				.FirstOrDefaultAsync();

			if (algorithm == null)
			{
				throw new ResourceAlreadyNotExistException(request.AlogrithmId, "Algorithm");
			}

			_algorithmTaskRepository.DeleteEntity(algorithm);
			await _algorithmTaskRepository.SaveChangesAsync();
		}
	}
}
