using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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

		public QuizApplicationDbContext(DbContextOptions<QuizApplicationDbContext> options)
			: base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuizApplicationDbContext).Assembly);

			base.OnModelCreating(modelBuilder);
		}
	}
}
