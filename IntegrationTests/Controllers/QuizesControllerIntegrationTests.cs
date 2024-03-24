using Application.Dto;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Newtonsoft.Json;

namespace IntegrationTests.Controllers
{
	public class QuizesControllerIntegrationTests : IntegrationTestsBase
	{
		[Fact]
		public async Task Controller_GetQuizes_ShouldReturn200StatusCode()
		{
			var reqeust = await _httpClient.GetAsync("api/quizes");

			reqeust.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.OK);
		}

		[Fact]
		public async Task Controller_GetQuizes_ShouldReturn200StatusCodeWithProperlySortedAndSearchedQuizes()
		{
			List<Quiz> quizzes = [
				new Quiz(Guid.NewGuid(), "", "Java", AdvanceNumber.Create(5)!) ,
				new Quiz(Guid.NewGuid(), "", "C#", AdvanceNumber.Create(4)!) ,
				new Quiz(Guid.NewGuid(), "", "JavaScript", AdvanceNumber.Create(7)!) ,
				new Quiz(Guid.NewGuid(), "", "PHP", AdvanceNumber.Create(1)!)
				];

			DbSeeder(db =>
			{
				db.Quizzes.AddRange(quizzes);
			});

			var reqeust = await _httpClient.GetAsync("api/quizes?SortColumn=technologyName&sortOrder=asc&SearchQuery=Java");

			var content = JsonConvert.DeserializeObject<List<QuizBasicResponseDto>>
				(await reqeust.Content.ReadAsStringAsync());	

			reqeust.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.OK);

			content!.Count
				.Should()
				.Be(2);

			content![0].TechnologyName
				.Should()
				.Be("Java");

			content![1].TechnologyName
				.Should()
				.Be("JavaScript");
		}

		[Fact]
		public async Task Controller_GetQuiz_ShouldReturn404StatusCode_WhenQuizNotFound()
		{
			var request = await _httpClient.GetAsync($"api/quizes/{Guid.NewGuid()}");

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.NotFound);	
		}

		[Fact]	
		public async Task Controller_GetQuiz_ShouldReturn200StatusCodeWithProperlyMappedQuiz_WhenQuizFound()
		{
			var id = Guid.NewGuid();
			var title = "SimpleQUiz";
			var technologyName = "Cobol";
			var advanceNumber = AdvanceNumber.Create(5)!;

			DbSeeder(db =>
			{
				db.Quizzes.Add(new Quiz(id , title , technologyName , advanceNumber));
				db.Quizzes.Add(new Quiz(Guid.NewGuid(), "fake", "fake", AdvanceNumber.Create(1)!));
			});

			var reqeust = await _httpClient.GetAsync($"api/quizes/{id}");

			var content = JsonConvert.DeserializeObject<QuizDetailResponseDto>
				(await reqeust.Content.ReadAsStringAsync());

			reqeust.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.OK);

			content!.Title
				.Should()
				.Be(title);

			content.AdvanceNumber
				.Should()
				.Be(advanceNumber.Number);

			content.TechnologyName
				.Should()
				.Be(technologyName);
		}
	}
}
