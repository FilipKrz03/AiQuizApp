using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
	public class AlgorithmAnswerResponseDto
	{
		public Guid Id {  get; set; }
		public string AnswerContent { get; set; } = string.Empty;
		public string Language { get; set; } = string.Empty;
	}
}
