using Application.Cqrs.UserAlgorithm.Command.CreateAlgorithm;
using Application.Extensions;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
	[Route("api/user/algorithms")]
	[ApiController]
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
	}
}
