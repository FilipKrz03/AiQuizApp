using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserAlgorithm.Command.CreateAlgorithm
{
	public sealed record CreateAlgorithmReqeust
	(string TaskTitle, string TaskMainTopics, int AdvanceNumber)
	{ }
}
