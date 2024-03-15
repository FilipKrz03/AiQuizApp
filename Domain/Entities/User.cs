using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public sealed class User : IdentityUser
	{
		public List<UserOwnQuiz> UserQuizzes { get; set; } = [];
		public List<UserOwnAlgorithmTask> UserAlgorithms { get; set; } = [];
	}
}
