using Application.Cqrs.UserQuiz.Command.CreateQuizCommand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
	[Route("api/user/quizes")]
	[Authorize]
	[ApiController]
	public class UserQuizesController(IMediator mediator) : ControllerBase
	{
		private readonly IMediator _mediator = mediator;

		[HttpPost]
		public async Task<ActionResult> CreateQuiz([FromBody] CreateAiQuizCommand command)
		{
			var result = await _mediator.Send(command);

			return Ok(result);
		}
	}
}
