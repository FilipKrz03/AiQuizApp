using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public class UserOwnQuiz : Quiz
	{
		[ForeignKey("UserId")]
		public string UserId { get; set; } = null!;
		public User User { get; set; } = null!;

		public UserOwnQuiz(Guid id, string title, string technologyName, AdvanceNumber advanceNumber , string userId)
			: base(id, title, technologyName, advanceNumber)
		{
			UserId = userId;
		}

		public UserOwnQuiz(Guid id, string title, string technologyName, AdvanceNumber advanceNumber)
			: base(id, title, technologyName, advanceNumber)
		{
		}
	}
}
