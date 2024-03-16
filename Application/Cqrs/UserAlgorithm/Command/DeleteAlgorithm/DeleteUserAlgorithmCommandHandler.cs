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
		IRepository<UserOwnAlgorithmTask> userOwnAlgorithmTaskRepository,
		IUserRepository userRepository
		) : IRequestHandler<DeleteUserAlgorithmCommand>
	{
		private readonly IRepository<UserOwnAlgorithmTask> _userOwnAlgorithmTaskRepository = userOwnAlgorithmTaskRepository;
		private readonly IUserRepository _userRepository = userRepository;

		public async Task Handle(DeleteUserAlgorithmCommand request, CancellationToken cancellationToken)
		{
			if (!await _userRepository.UserExistAsync(request.UserId))
			{
				throw new InvalidTokenClaimException();
			}

			var algorithm = await _userOwnAlgorithmTaskRepository
				.GetByIdQuery(request.AlogrithmId)
				.Where(x => x.UserId == request.UserId)
				.FirstOrDefaultAsync();

			if (algorithm == null)
			{
				throw new ResourceAlreadyNotExistException(request.AlogrithmId, "Algorithm");
			}

			_userOwnAlgorithmTaskRepository.DeleteEntity(algorithm);
			await _userOwnAlgorithmTaskRepository.SaveChangesAsync();
		}
	}
}
