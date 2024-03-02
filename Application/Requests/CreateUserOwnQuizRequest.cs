﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Requests
{
	public sealed record CreateUserOwnQuizRequest(string TechnologyName, int AdvanceNumber, string? QuizTitle)
	{
	}
}
