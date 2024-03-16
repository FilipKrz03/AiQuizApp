using Application.Common;
using Application.Cqrs.Algorithm.Query.GetAlgorithm;
using Application.Cqrs.UserAlgorithm.Command.CreateAlgorithm;
using Application.Cqrs.UserAlgorithm.Command.DeleteAlgorithm;
using Application.Cqrs.UserAlgorithm.Query.GetAlgorithm;
using Application.Cqrs.UserAlgorithm.Query.GetAlgorithms;
using Application.Dto;
using Application.Extensions;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
	[Route("api/user/algorithms")]
	[ApiController]
	[Authorize]
	public class UserAlgorithmsController(IMediator mediator) : ControllerBase
	{
		private readonly IMediator _mediator = mediator;

		[HttpPost]
		public async Task<ActionResult<string>> CreateAlgorithm([FromBody] CreateAlgorithmReqeust reqeust)
		{
			var result = await _mediator.Send(new CreateAlgorithmCommand(
				User.Claims.GetId(),
				reqeust.TaskTitle,
				reqeust.TaskMainTopics,
				AdvanceNumber.Create(reqeust.AdvanceNumber)!
				));

			return Ok(result);
		}

		[HttpGet]
		public async Task<ActionResult<PagedList<AlgorithmTaskBasicResponseDto>>> GetAlgortihms([FromQuery] ResourceParamethers resourceParamethers)
		{
			var result = await _mediator.Send(new GetUserAlgorithmsQuery(User.Claims.GetId() , resourceParamethers));

			Response.AddPaginationHeader(result);	

			return Ok(result);
		}

		[HttpGet("{algorithmId}")]
		public async Task<ActionResult<AlgorithmTaskDetailResponseDto>> GetAlgorithm(Guid algorithmId)
		{
			var result = await _mediator.Send(new GetUserAlgorithmQuery(User.Claims.GetId(), algorithmId));

			return Ok(result);
		}

		[HttpDelete("{algorithmId}")]
		public async Task<ActionResult> DeleteAlgorithm(Guid algorithmId)
		{
			await _mediator.Send(new DeleteUserAlgorithmCommand(User.Claims.GetId(), algorithmId));

			return NoContent();
		}
	}
}
