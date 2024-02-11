﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
	public class AnswerResponseDto
	{
		public Guid Id { get; set; }
		public string Content { get; set; } = string.Empty;
		public char AnswerLetter { get; set; }	
	}
}
