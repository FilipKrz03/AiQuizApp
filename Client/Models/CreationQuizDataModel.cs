using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
	public class CreationQuizDataModel
	{
		[Required (ErrorMessage = "Tytuł quizu jest wymagany")]
		public string QuizTitle { get; set; } = string.Empty;

		[Required(ErrorMessage = "Technologia quizu jest wymagana")]	
		public string TechnologyName { get; set; } = string.Empty;

		[Required(ErrorMessage = "Poziom zaawansowania jest wymagany")
		, Range(1, 10, ErrorMessage = "Poziom zaawansowania powinien być miedzy 1 a 10")]
		public int AdvanceNumber { get; set; } = 4;
	}
}
