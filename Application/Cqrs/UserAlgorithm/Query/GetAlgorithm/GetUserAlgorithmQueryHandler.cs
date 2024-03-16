using Application.Dto;
using AutoMapper;
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

namespace Application.Cqrs.UserAlgorithm.Query.GetAlgorithm
{
	public sealed class GetUserAlgorithmQueryHandler(
		IUserRepository userRepository , 
		IRepository<UserOwnAlgorithmTask> userOwnAlgorithmTaskRepository , 
		IMapper mapper
		) : IRequestHandler<GetUserAlgorithmQuery, AlgorithmTaskDetailResponseDto>
	{
		private readonly IUserRepository _userRepository = userRepository;
		private readonly IRepository<UserOwnAlgorithmTask> _userOwnAlgorithmTaskRepository = userOwnAlgorithmTaskRepository;
		private readonly IMapper _mapper = mapper;

		public async Task<AlgorithmTaskDetailResponseDto> Handle(GetUserAlgorithmQuery request, CancellationToken cancellationToken)
		{
			if(!await _userRepository.UserExistAsync(request.UserId))
			{
				throw new InvalidTokenClaimException();
			}

			var algorithm = await _userOwnAlgorithmTaskRepository
				.GetByIdQuery(request.AlgorithmId)
				.Where(x => x.UserId == request.UserId)
				.Include(x => x.Answers)
				.FirstOrDefaultAsync();

			if(algorithm == null)
			{
				throw new ResourceNotFoundException(request.AlgorithmId, "Algorithm");
			}

			return _mapper.Map<AlgorithmTaskDetailResponseDto>(algorithm)!;
		}
	}
}
