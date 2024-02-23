﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public sealed class User : IdentityUser
	{
		public List<Quiz> Quizzes { get; set; } = [];
	}
}