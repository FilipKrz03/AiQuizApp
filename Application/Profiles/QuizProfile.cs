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
    public class QuizProfile : Profile
    {
        public QuizProfile()
        {
            CreateMap<Quiz, QuizDetailResponseDto>()
                .ForMember(src =>
                     src.AdvanceNumber, opt => opt.MapFrom(x => x.AdvanceNumber.Number));
            CreateMap<Quiz, QuizBasicResponseDto>();
        }
    }
}
