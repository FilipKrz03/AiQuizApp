using Application.Dto;
using Application.Requests;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Newtonsoft.Json;

namespace IntegrationTests.Controllers
{
	public class UserQuizesControllerIntegrationTests : IntegrationTestsBase
	{
		[Fact]
		public async Task Controller_CreateQuiz_ShouldReturn400StatusCode_WhenUserFromTokenClaimDoNotExist()
		{
			SetUserId("cq1");

			var request = await _httpClient.PostAsJsonAsync("api/user/quizes", new CreateUserQuizRequest("", 5, ""));

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task Controller_CreateQuiz_ShouldReturn404StatusCode_WhenUserExistAndRequestIsNotProper()
		{
			var userId = "cq2";

			CreateUserWithId(userId);

			var request = await _httpClient
				.PostAsJsonAsync("api/user/quizes", new CreateUserQuizRequest("someTech", 12, "someValue"));
			// Advance number shoould be > 0 and < 10 - so this is bad request

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task Controller_CreateQuiz_ShouldReturn200StatusCode_WhenUserExistAndRequestIsProper()
		{
			var userId = "cq3";

			CreateUserWithId(userId);

			var request = await _httpClient.PostAsJsonAsync("api/user/quizes", new CreateUserQuizRequest("someTech", 5, "someValue"));

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.OK);
		}

		[Fact]
		public async Task Controller_GetQuiz_ShouldReturn400StatusCode_WhenUserFromTokenClaimDoNotExist()
		{
			SetUserId("gq1");

			var request = await _httpClient.PostAsJsonAsync("api/user/quizes", new CreateUserQuizRequest("", 5, ""));

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.BadRequest);
		}


		[Fact]
		public async Task Controller_GetQuiz_Should_Return404StatusCode_WhenUserExistAndUserQuizDoNotExist()
		{
			var userId = "gq2";

			CreateUserWithId(userId);

			var request = await _httpClient.GetAsync($"api/user/quizes/{Guid.NewGuid()}");

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task Controller_GetQuiz_Should_Return200StatusCodeWithProperlyMappedQuiz_WhenUserExistAndQuizExist()
		{
			string userId = "gq3";
			var quizId = Guid.NewGuid();

			CreateUserWithId(userId);

			DbSeeder(db =>
			{
				db.UserOwnQuizzes.Add(new(quizId, "testTitle", "testTechnology", AdvanceNumber.Create(5)!, userId));
			});

			var request = await _httpClient.GetAsync($"api/user/quizes/{quizId}");

			var content = JsonConvert.DeserializeObject<QuizDetailResponseDto>
				(await request.Content.ReadAsStringAsync());

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.OK);

			content!.AdvanceNumber
				.Should()
				.Be(5);

			content!.Title
				.Should()
				.Be("testTitle");

			content!.TechnologyName
				.Should()
				.Be("testTechnology");
		}

		[Fact]
		public async Task Controller_GetQuizes_ShouldReturn400StatusCode_WhenUserFromTokenClaimDoNotExist()
		{
			var request = await _httpClient.GetAsync("api/user/quizes");

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task Controller_GetQuizes_ShouldReturn200StatusCodeWithProperlySearchedAndFilteredQuizes_WhenUserExist()
		{
			string userId = "cqz1";

			CreateUserWithId(userId);

			List<UserOwnQuiz> quizzes = [
				new UserOwnQuiz(Guid.NewGuid(), "", "Java", AdvanceNumber.Create(5)!, userId),
				new UserOwnQuiz(Guid.NewGuid(), "", "C#", AdvanceNumber.Create(4)! , userId),
				new UserOwnQuiz(Guid.NewGuid(), "", "JavaScript", AdvanceNumber.Create(7)! , userId),
				new UserOwnQuiz(Guid.NewGuid(), "", "PHP", AdvanceNumber.Create(1)! , userId), 
				new UserOwnQuiz(Guid.NewGuid(), "", "JavaFx", AdvanceNumber.Create(1)!, "Unkownuser") // Should not be included (diffrent userId)
			];

			DbSeeder(db =>
			{
				db.UserOwnQuizzes.AddRange(quizzes);
			});

			var reqeust = await _httpClient.GetAsync("api/user/quizes?SortColumn=technologyName&sortOrder=asc&SearchQuery=Java");

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
		public async Task Controller_DeleteQuiz_ShouldReturn400StatusCode_WhenUserFromTokenClaimDoNotExist()
		{
			var request = await _httpClient.DeleteAsync($"api/user/quizes/{Guid.NewGuid()}");

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task Controller_DeleteQuiz_Should_Return404StatusCode_WhenQuizToDeleteNotExist()
		{
			CreateUserWithId("dq1");

			var request = await _httpClient.DeleteAsync($"api/user/quizes/{Guid.NewGuid()}");

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task Controller_DeleteQuiz_Should_Return204StatusCodeAndDeleteQuiz_WhenQuizToDeleteExist()
		{
			string userId = "dq2";
			var quizId = Guid.NewGuid();	

			CreateUserWithId(userId);

			DbSeeder(db => db.UserOwnQuizzes.Add(new(quizId, "", "", AdvanceNumber.Create(5)!, userId)));

			var request = await _httpClient.DeleteAsync($"api/user/quizes/{quizId}");

			var deletedQuiz = DbContextGetter().UserOwnQuizzes
				.Where(x => x.Id == quizId)
				.FirstOrDefault();

			deletedQuiz
				.Should()
				.BeNull();

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.NoContent);
		}
	}
}
