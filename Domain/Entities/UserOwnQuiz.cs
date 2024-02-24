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
		public UserOwnQuiz
			(Guid id, string title, string technologyName, AdvanceNumber advanceNumber, string userId )
			: base(id, title, technologyName, advanceNumber)
		{
			UserId = userId;
		}

		[ForeignKey("UserId")]
		public string UserId { get; set; }
		public User User { get; set; } = null!;
	}
}
