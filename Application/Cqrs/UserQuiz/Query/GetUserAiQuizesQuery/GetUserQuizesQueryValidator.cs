﻿using Application.Cqrs.UserAlgorithm.Query.GetAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Cqrs.UserQuiz.Query.GetUserAiQuizesQuery
{
	public class GetUserQuizesQueryValidator : CreationStatusFilterCorrectnessValidator<GetUserQuizesQuery>
	{
		// All work is done by CreationStatusFilterCorrectnesValidator generic class
	}
}
