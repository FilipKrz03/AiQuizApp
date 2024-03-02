using Application.Cqrs.UserQuiz.Command.CreateAiQuiz;
using Application.Cqrs.UserQuiz.Query.GetUserAiQuizQuery;
using Application.Extensions;
using Application.Requests;
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
		public async Task<ActionResult> CreateQuiz([FromBody] CreateUserOwnQuizRequest request)
		{
			var result = await _mediator.Send
				(
					new CreateAiQuizCommand(
						User.Claims.GetId(),
						request.TechnologyName,
						request.AdvanceNumber,
						request.QuizTitle
						)
				);

			return Ok(result);
		}

		[HttpGet("{quizId}")]
		public async Task<ActionResult> GetQuiz(Guid quizId)
		{
			var result = await _mediator.Send(new GetUserAiQuizQuery(quizId, User.Claims.GetId()));

			return Ok(result);
		}
	}
}
