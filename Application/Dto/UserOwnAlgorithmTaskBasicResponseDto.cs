using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
	public class UserOwnAlgorithmTaskBasicResponseDto : AlgorithmTaskBasicResponseDto
	{
		public CreationStatus CreationStatus { get; set; }
	}
}
