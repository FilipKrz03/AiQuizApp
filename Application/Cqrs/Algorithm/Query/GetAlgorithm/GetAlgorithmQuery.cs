using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.Algorithm.Query.GetAlgorithm
{
	public sealed record GetAlgorithmQuery(Guid AlgorithmId) : IRequest<AlgorithmTaskDetailResponseDto>
	{
	}
}
