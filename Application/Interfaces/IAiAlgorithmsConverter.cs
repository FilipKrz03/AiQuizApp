﻿using Application.Dto;
using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface IAiAlgorithmsConverter
	{
		AlgorithmTask ConvertToAlgorithmTask(AlgorithmAiResponseDto response, string taskTitle, AdvanceNumber advanceNumber);
	}
}
