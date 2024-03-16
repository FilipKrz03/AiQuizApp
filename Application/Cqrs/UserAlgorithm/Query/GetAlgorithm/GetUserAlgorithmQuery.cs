using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserAlgorithm.Query.GetAlgorithm
{
	public sealed record GetUserAlgorithmQuery(string UserId , Guid AlgorithmId) : IRequest<AlgorithmTaskDetailResponseDto>
	{
	}
}
