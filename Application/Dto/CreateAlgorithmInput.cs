using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
	public sealed record CreateAlgorithmInput(AdvanceNumber advanceNumber, string taskTitle, string specialTopics)
	{
	}
}
