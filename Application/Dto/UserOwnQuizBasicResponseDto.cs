using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
	public class UserOwnQuizBasicResponseDto : QuizBasicResponseDto
	{
		public CreationStatus CreationStatus { get; set; }	
	}
}
