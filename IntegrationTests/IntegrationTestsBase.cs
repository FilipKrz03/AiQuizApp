using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IntegrationTests
{
	public abstract class IntegrationTestsBase
	{
		protected readonly WebApplicationFactory<Program> _factory;
		protected readonly HttpClient _httpClient;

		protected IntegrationTestsBase()
		{
			var appFactory = new WebApplicationFactory<Program>()
				.WithWebHostBuilder(builder =>
				{
					builder.ConfigureTestServices(services =>
					{
						services.RemoveAll(typeof(IHostedService));
						services.RemoveAll(typeof(QuizApplicationDbContext));

						services.AddDbContext<QuizApplicationDbContext>(options =>
						{
							options.UseInMemoryDatabase("TestDb");
						});

						var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<QuizApplicationDbContext>));

						if (descriptor != null)
						{
							services.Remove(descriptor);
						}

						services.AddDbContext<QuizApplicationDbContext>(options =>
						{
							options.UseInMemoryDatabase("TestDb");
						});

						services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
						 .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });
					});
				});

			_factory = appFactory;
			_httpClient = appFactory.CreateClient();

			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Test");
		}

		protected void DbSeeder(Action<QuizApplicationDbContext> dbAction)
		{
			using (var scope = _factory.Services.CreateScope())
			{
				var scopedServices = scope.ServiceProvider;
				var db = scopedServices.GetRequiredService<QuizApplicationDbContext>();

				dbAction.Invoke(db);

				db.SaveChanges();
			}
		}

		protected void SetUserId(string id)
		{
			_httpClient.DefaultRequestHeaders.Add("userId", id);
		}

		protected void CreateUserWithId(string userId)
		{
			SetUserId(userId);

			DbSeeder(db => db.Users.Add(new() { Id = userId }));
		}

		protected QuizApplicationDbContext DbContextGetter()
		{
			var scope = _factory.Services.CreateScope();

			var scopedServices = scope.ServiceProvider;
			var db = scopedServices.GetRequiredService<QuizApplicationDbContext>();

			return db;
		}
	}
}
