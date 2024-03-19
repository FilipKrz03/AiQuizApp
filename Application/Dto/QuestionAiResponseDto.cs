using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
	public class QuestionAiResponseDto
	{
		public string QuestionContent { get; set; } = string.Empty;
		public IEnumerable<string> Answers { get; set; } = [];
		public char CharOfProperAnswer { get; set; }
	}
}
