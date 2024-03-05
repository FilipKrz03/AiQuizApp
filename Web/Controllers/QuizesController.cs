using Application.Common;
using Application.Cqrs.Quiz.Query.GetQuiz;
using Application.Cqrs.Quiz.Query.GetQuizes;
using Application.Dto;
using Application.Extensions;
using Azure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/quizes")]
    [ApiController]
    public sealed class QuizesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{quizId}")]
        public async Task<ActionResult<QuizDetailResponseDto>> GetQuizDetails(Guid quizId)
        {
            var result = await _mediator.Send(new GetQuizQuery(quizId));

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<QuizBasicResponseDto>>>
            GetQuizes([FromQuery]ResourceParamethers resourceParamethers)
        {
            var result = await _mediator.Send(new GetQuizesQuery(resourceParamethers));

            Response.AddPaginationHeader(result);

            return Ok(result);
        }
    }
}
