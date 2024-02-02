using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DbContexts
{
    public class QuizApplicationDbContext : DbContext
    {
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public QuizApplicationDbContext(DbContextOptions<QuizApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .Property(p => p.ProperAnswerNumber)
                .HasConversion(
                ProperAnswerNumber => ProperAnswerNumber.Number,
                value => ProperAnswerNumber.Create(value)!
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
