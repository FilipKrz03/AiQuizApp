﻿using Domain.Entities;
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

        public QuizApplicationDbContext(DbContextOptions<QuizApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .Property(p => p.ProperAnswerLetter)
                .HasConversion(
                ProperAnswerLetter => ProperAnswerLetter.Letter,
                value => AnswerLetter.Create(value)!
                );

            modelBuilder.Entity<Quiz>()
                .Property(p => p.AdvanceNumber)
                .HasConversion(
                 AdvanceNumber => AdvanceNumber.Number,
                 value => AdvanceNumber.Create(value)!
                 );

			modelBuilder.Entity<UserOwnQuiz>()
			   .Property(p => p.AdvanceNumber)
			   .HasConversion(
				AdvanceNumber => AdvanceNumber.Number,
				value => AdvanceNumber.Create(value)!
				);

			modelBuilder.Entity<Answer>()
                .Property(p => p.AnswerLetter)
                .HasConversion(
                 AnswerLetter => AnswerLetter.Letter,
                 value => AnswerLetter.Create(value)!
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
