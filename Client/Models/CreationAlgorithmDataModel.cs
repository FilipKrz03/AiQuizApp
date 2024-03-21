using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
	public class CreationAlgorithmDataModel 
	{
		[Required(ErrorMessage = "Musisz podac tytuł zdania")]
		public string TaskTitle { get; set; } = string.Empty;

		[Required(ErrorMessage = "Musisz podać główne zagadnienia zadania")]
		public string TaskMainTopics { get; set; } = string.Empty;

		[Required(ErrorMessage = "Poziom zaawansowania jest wymagany")
			, Range(1 , 10 , ErrorMessage = "Poziom zaawansowania powinien być miedzy 1 a 10")]
		public int AdvanceNumber { get; set; } = 4;
	}
}
