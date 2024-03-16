using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserAlgorithm.Command.DeleteAlgorithm
{
	public sealed record DeleteUserAlgorithmCommand(string UserId , Guid AlogrithmId) : IRequest
	{
	}
}
