using Application.Common;
using Application.Dto;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserAlgorithm.Query.GetAlgorithms
{
	public sealed record GetUserAlgorithmsQuery(string UserId , ResourceParamethersWithCreationStatus ResourceParamethers) 
		: IRequest<PagedList<UserOwnAlgorithmTaskBasicResponseDto>> , IResourceParamethersWithCreationStatus
	{
	}
}
