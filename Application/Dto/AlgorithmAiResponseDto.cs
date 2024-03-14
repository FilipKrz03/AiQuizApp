using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
	public class AlgorithmAiResponseDto
	{
		public string QuestionContent { get; set; } = string.Empty;
		public IEnumerable<ProgrammingLanguageWithAnswerAiResponseDto> Answers{ get; set; } = [];
	}
}
