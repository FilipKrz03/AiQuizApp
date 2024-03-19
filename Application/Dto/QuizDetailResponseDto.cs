using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
	public class QuizDetailResponseDto
	{
		public string Title { get; set; } = string.Empty;
		public string TechnologyName { get; set; } = string.Empty;
		public int AdvanceNumber { get; set; }
		public IEnumerable<QuestionResponseDto> Questions { get; set; } = [];
	}
}
