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
	public class UserOwnQuizConfiguration : IEntityTypeConfiguration<UserOwnQuiz>
	{
		public void Configure(EntityTypeBuilder<UserOwnQuiz> builder)
		{
			builder
			 .Property(p => p.AdvanceNumber)
			 .HasConversion(
				   AdvanceNumber => AdvanceNumber.Number,
				   value => AdvanceNumber.Create(value)!
				   );

			builder.ToTable("UserOwnQuizzes");
		}
	}
}
