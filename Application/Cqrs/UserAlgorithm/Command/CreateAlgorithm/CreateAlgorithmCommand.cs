using Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserAlgorithm.Command.CreateAlgorithm
{
	public sealed record CreateAlgorithmCommand 
		(string UserId , string TaskTitle , string TaskMainTopics, AdvanceNumber AdvanceNumber) : IRequest<string>
	{
	}
}
