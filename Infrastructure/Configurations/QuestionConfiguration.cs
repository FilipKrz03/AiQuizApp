using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
	public class QuestionConfiguration : IEntityTypeConfiguration<Question>
	{
		public void Configure(EntityTypeBuilder<Question> builder)
		{
			builder
			.Property(p => p.ProperAnswerLetter)
			.HasConversion(
				ProperAnswerLetter => ProperAnswerLetter.Letter,
				value => AnswerLetter.Create(value)!
				);

			builder
			.HasOne(x => x.Quiz)
			.WithMany(x => x.Questions)
			.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
