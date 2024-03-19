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
	public class QuestionProfile : Profile
	{
		public QuestionProfile()
		{
			CreateMap<Question, QuestionResponseDto>()
				.ForMember(src => src.ProperAnswerLetter, opt => opt.MapFrom(x => x.ProperAnswerLetter.Letter));
		}
	}
}
