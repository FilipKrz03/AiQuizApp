using Application.Dto;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles
{
	public class AnswerProfile : Profile
	{
		public AnswerProfile()
		{
			CreateMap<Answer, AnswerResponseDto>()
					.ForMember(src => src.AnswerLetter, opt => opt.MapFrom(x => x.AnswerLetter.Letter));
		}
	}
}
