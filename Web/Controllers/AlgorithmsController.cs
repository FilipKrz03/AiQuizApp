using Application.Common;
using Application.Cqrs.Algorithm.Query.GetAlgorithm;
using Application.Cqrs.Algorithm.Query.GetAlgorithms;
using Application.Dto;
using Application.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
	[Route("api/algorithms")]
	[ApiController]
	public class AlgorithmsController(IMediator mediator) : ControllerBase
	{
		private readonly IMediator _mediator = mediator;

		[HttpGet("{algorithmId}")]
		public async Task<ActionResult<AlgorithmTaskDetailResponseDto>> GetAlgorithm(Guid algorithmId)
		{
			var result = await _mediator.Send(new GetAlgorithmQuery(algorithmId));

			return Ok(result);	
		}

		[HttpGet]
		public async Task<ActionResult<PagedList<AlgorithmTaskBasicResponseDto>>>
			GetAlgorithms([FromQuery]ResourceParamethers resourceParamethers)
		{
			var result = await _mediator.Send(new GetAlgorithmsQuery(resourceParamethers));

			Response.AddPaginationHeader(result);

			return Ok(result);
		}
	}
}
