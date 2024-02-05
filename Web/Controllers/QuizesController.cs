using Application.Cqrs.Quiz.Query.GetQuiz;
using Application.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/quizes")]
    [ApiController]
    public class QuizesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuizesController(IMediator mediator) => _mediator = mediator;

        [HttpGet("{quizId}")]
        public async Task<QuizDetailResponseDto> GetQuizDetails(Guid quizId)
        {
            var result = await _mediator.Send(new GetQuizQuery(quizId));

            return result;
        }
    }
}
