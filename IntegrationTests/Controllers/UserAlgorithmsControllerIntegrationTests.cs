using Application.Cqrs.UserAlgorithm.Command.CreateAlgorithm;
using Application.Dto;
using Application.Requests;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Newtonsoft.Json;

namespace IntegrationTests.Controllers
{
	public class UserAlgorithmsControllerIntegrationTests : IntegrationTestsBase
	{
		[Fact]
		public async Task Controller_CreateAlgorithm_ShouldReturn400StatusCode_WhenUserFromTokenClaimDoNotExist()
		{
			var request = await _httpClient.PostAsJsonAsync("api/user/algorithms", new CreateAlgorithmReqeust("", "", 5));

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task Controller_CreateAlgorithm_ShouldReturn404StatusCode_WhenUserExistAndNotProperRequest()
		{
			{
				CreateUserWithId("ca1");

				var request = await _httpClient.PostAsJsonAsync("api/user/algorithms", new CreateAlgorithmReqeust("", "", 5));
				// Task main topics should not be empty so reuqest is not proper

				request.StatusCode
					.Should()
					.Be(System.Net.HttpStatusCode.BadRequest);
			}
		}

		[Fact]
		public async Task Controller_CreateAlgorithm_ShouldReturn200StatusCode_WhenUserExistAndProperRequest()
		{
			CreateUserWithId("ca2");

			var request = await _httpClient.PostAsJsonAsync("api/user/algorithms", new CreateAlgorithmReqeust("Proper", "Proper", 5));

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.OK);
		}

		[Fact]
		public async Task Controller_GetAlgorithm_ShouldReturn400StatusCode_WhenUserFromTokenClaimDoNotExist()
		{
			var request = await _httpClient.GetAsync($"api/user/algorithms/{Guid.NewGuid()}");

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task Controller_GetAlgorithm_ShouldReturn404StatusCode_WhenUserExistAndAlgorithmDoNotExist()
		{
			CreateUserWithId("ga1");

			var request = await _httpClient.GetAsync($"api/user/algorithms/{Guid.NewGuid()}");

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task Controller_GetAlgorithm_ShouldReturn200StatusCodeWithProperlyMappedAlgorithm_WhenUserAndAlgorithmExist()
		{
			var userId = "ga2";
			var algorithmId = Guid.NewGuid();

			CreateUserWithId(userId);

			DbSeeder(db => db.UserOwnAlgorithms.Add(new(algorithmId , "taskTitle", "taskTopic", "", AdvanceNumber.Create(5)!, userId)));

			var request = await _httpClient.GetAsync($"api/user/algorithms/{algorithmId}");

			var result = JsonConvert.DeserializeObject<AlgorithmTaskDetailResponseDto>
				(await request.Content.ReadAsStringAsync());

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.OK);

			result!.TaskTitle
				.Should()
				.Be("taskTitle");

			result!.TaskMainTopics
				.Should()
				.Be("taskTopic");

			result!.AdvanceNumber
				.Should()
				.Be(5);
		}

		[Fact]
		public async Task Controller_GetAlgorithms_ShouldReturn400StatusCode_WhenUserFromTokenClaimDoNotExist()
		{
			var request = await _httpClient.GetAsync("api/user/algorithms");

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task Controller_GetAlgorithms_Shold_ReturnProperlySortedAndSearchedAlgorithms()
		{
			string userId = "gas1";

			List<UserOwnAlgorithmTask> algoritms = [
				new UserOwnAlgorithmTask(Guid.NewGuid(), "BubbleSorting", "Test", "Test", AdvanceNumber.Create(5)! , userId),
				new UserOwnAlgorithmTask(Guid.NewGuid(), "Numbers", "Test", "Test", AdvanceNumber.Create(5)! , userId),
				new UserOwnAlgorithmTask(Guid.NewGuid(), "QuickSorting", "Test", "Test", AdvanceNumber.Create(5)! , userId),
				new UserOwnAlgorithmTask(Guid.NewGuid(), "SuperSorting", "Test", "Test", AdvanceNumber.Create(5)!, "Unkown"), // should not be included
			];

			CreateUserWithId(userId);
			DbSeeder(db => db.AddRange(algoritms));

			var result = await _httpClient.GetAsync("api/user/algorithms?SearchQuery=Sorting&SortColumn=taskTitle&SortOrder=desc");

			var content = JsonConvert.DeserializeObject<List<UserOwnAlgorithmTaskBasicResponseDto>>
				(await result.Content.ReadAsStringAsync());

			content!.Count
				.Should()
				.Be(2);

			content[0].TaskTitle
				.Should()
				.Be("QuickSorting");

			content[1].TaskTitle
				.Should()
				.Be("BubbleSorting");
		}

		[Fact]
		public async Task Controller_DeleteAlgorithm_ShouldReturn400StatusCode_WhenUserFromTokenClaimDoNotExist()
		{
			var request = await _httpClient.DeleteAsync($"api/user/algorithms/{Guid.NewGuid()}");

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task Controller_DeleteAlgorithm_ShouldReturn404StatusCode_WhenUserExistAndAlgorithmToDeleteDoNotExist()
		{
			CreateUserWithId("da1");

			var request = await _httpClient.DeleteAsync($"api/user/algorithms/{Guid.NewGuid()}");

			request.StatusCode
			.Should()
			.Be(System.Net.HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task Controller_DeleteAlgorithm_ShouldReturn204StatusCodeAndDeleteAlgorithm_WhenUserAndAlgorithmToDeleteExist()
		{
			var userId = "da2";
			var algorithmId = Guid.NewGuid();

			CreateUserWithId(userId);
			DbSeeder(db => db.UserOwnAlgorithms.Add(new(algorithmId, "taskTitle", "taskTopic", "", AdvanceNumber.Create(5)!, userId)));

			var reqeust = await _httpClient.DeleteAsync($"api/user/algorithms/{algorithmId}");

			var deletedAlgorithm = DbContextGetter().UserOwnAlgorithms
				.Where(x => x.Id == algorithmId)
				.FirstOrDefault();

			deletedAlgorithm
				.Should()
				.BeNull();

			reqeust.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.NoContent);
		}
	}
}
