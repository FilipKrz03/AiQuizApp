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
	public class AlgorithmTaskProfile : Profile
	{
		public AlgorithmTaskProfile()
		{
			CreateMap<AlgorithmTask, AlgorithmTaskDetailResponseDto>()
				.ForMember(x => x.AdvanceNumber, opt => opt.MapFrom(src => src.AdvanceNumber.Number));
			CreateMap<AlgorithmTask, AlgorithmTaskBasicResponseDto>()
				.ForMember(x => x.AdvanceNumber, opt => opt.MapFrom(src => src.AdvanceNumber.Number));
			CreateMap<UserOwnAlgorithmTask, UserOwnAlgorithmTaskBasicResponseDto>()
				.ForMember(x => x.AdvanceNumber, opt => opt.MapFrom(src => src.AdvanceNumber.Number));
		}
	}
}
