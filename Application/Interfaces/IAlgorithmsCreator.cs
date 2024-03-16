using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
	public interface IAlgorithmsCreator
	{
		Task<AlgorithmTask?> CreateAsync(AdvanceNumber advanceNumber, string taskTitle, string specialTopics);
		Task<(string, List<AlgorithmAnswer>)?>CreateAlgorithmContentAndAnswersAsync(
			AdvanceNumber advanceNumber,
			string specialTopics,
			Guid algorithmId
			);
	}
}
