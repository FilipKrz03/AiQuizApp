using Application.Common;
using Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.Algorithm.Query.GetAlgorithms
{
	public sealed record GetAlgorithmsQuery(ResourceParamethers ResourceParamethers) 
		: IRequest<PagedList<AlgorithmTaskBasicResponseDto>>
	{
	}
}
