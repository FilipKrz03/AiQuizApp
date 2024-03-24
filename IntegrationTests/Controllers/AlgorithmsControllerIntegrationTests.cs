using Application.Dto;
using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;
using Newtonsoft.Json;

namespace IntegrationTests.Controllers
{
	public class AlgorithmsControllerIntegrationTests : IntegrationTestsBase
	{
		[Fact]
		public async Task Controller_GetAlgorithms_ShouldReturn200StatusCode()
		{
			var reqeust = await _httpClient.GetAsync("api/algorithms");

			reqeust.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.OK);
		}

		[Fact]
		public async Task Controller_GetAlgorithms_Shold_ReturnProperlySortedAndSearchedAlgorithms()
		{
			List<AlgorithmTask> algoritms = [
				new AlgorithmTask(Guid.NewGuid(), "BubbleSorting", "Test", "Test", AdvanceNumber.Create(5)!),
				new AlgorithmTask(Guid.NewGuid(), "Numbers", "Test", "Test", AdvanceNumber.Create(5)!),
				new AlgorithmTask(Guid.NewGuid(), "QuickSorting", "Test", "Test", AdvanceNumber.Create(5)!),
				new AlgorithmTask(Guid.NewGuid(), "BinarySearch", "Test", "Test", AdvanceNumber.Create(5)!) ,
				new AlgorithmTask(Guid.NewGuid(), "BinaryTree", "Test", "Test", AdvanceNumber.Create(5)!)
			];

			DbSeeder(db => db.AddRange(algoritms));

			var result = await _httpClient.GetAsync("api/algorithms?SearchQuery=Binary&SortColumn=taskTitle&SortOrder=desc");

			var content = JsonConvert.DeserializeObject<List<AlgorithmTaskBasicResponseDto>>
				(await result.Content.ReadAsStringAsync());

			content!.Count
				.Should()
				.Be(2);

			content[0].TaskTitle
				.Should()
				.Be("BinaryTree");

			content[1].TaskTitle
				.Should()
				.Be("BinarySearch");
		}

		[Fact]
		public async Task Controller_GetAlgorithm_Should_ReturnResponseWith404Status_WhenAlgorithmNotFound()
		{
			DbSeeder(db => db.Add(new AlgorithmTask(Guid.NewGuid(), "BubbleSorting", "Test", "Test", AdvanceNumber.Create(5)!)));

			var request = await _httpClient.GetAsync($"api/algorithms/{Guid.NewGuid()}");

			request.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task Controller_GetAlgorithm_Should_Return200ResponseWithProperlyMappedAlgorithm_WhenAlgorithmFound()
		{
			var id = Guid.NewGuid();
			var title = "Sorting";
			var taskMainTopics = "mainTopicSimple";

			DbSeeder(db => db.Add(new AlgorithmTask(id, title, taskMainTopics, "", AdvanceNumber.Create(3)!)));

			var result = await _httpClient.GetAsync($"api/algorithms/{id}");

			var content = JsonConvert.DeserializeObject<AlgorithmTaskDetailResponseDto>
				(await result.Content.ReadAsStringAsync());

			result.StatusCode
				.Should()
				.Be(System.Net.HttpStatusCode.OK);

			content!.Id
				.Should()
				.Be(id);

			content!.TaskTitle
				.Should()
				.Be(title);

			content!.TaskMainTopics
				.Should()
				.Be(taskMainTopics);
		}
	}
}
