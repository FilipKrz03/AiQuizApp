using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
	public class CreationQuizDataModel
	{
		[Required]
		public string QuizTitle { get; set; } = string.Empty;

		[Required]	
		public string TechnologyName { get; set; } = string.Empty;
		public int AdvanceNumber { get; set; } = 4;
	}
}
