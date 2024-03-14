using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
	public class AlgorithmTaskBasicResponseDto
	{
		public Guid Id { get; set; }
		public string TaskTitle { get; set; } = string.Empty;
		public string TaskMainTopics { get; set; } = string.Empty;
		public string TaskContent { get; set; } = string.Empty;
		public int AdvanceNumber { get; set; }
	}
}
