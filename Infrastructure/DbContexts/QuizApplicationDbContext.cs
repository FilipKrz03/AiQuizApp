using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DbContexts
{
	public class QuizApplicationDbContext : IdentityDbContext<User>
	{
		public DbSet<Quiz> Quizzes { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<Answer> Answers { get; set; }
		public DbSet<UserOwnQuiz> UserOwnQuizzes { get; set; }
		public DbSet<AlgorithmTask> AlgorithmTasks { get; set; }
		public DbSet<AlgorithmAnswer> AlgorithmAnswers { get; set; }
		public DbSet<UserOwnAlgorithmTask> UserOwnAlgorithms { get; set; }

		public QuizApplicationDbContext(
			DbContextOptions<QuizApplicationDbContext> options,
			ILogger<QuizApplicationDbContext> logger
			) : base(options)
		{
			try
			{
				var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
				if (databaseCreator != null)
				{
					if (!databaseCreator.CanConnect()) databaseCreator.Create();
					if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
				}
			}
			catch (Exception ex)
			{
				logger.LogError("Quiz application db context - {ex}", ex);
				throw;
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuizApplicationDbContext).Assembly);

			base.OnModelCreating(modelBuilder);
		}
	}
}
