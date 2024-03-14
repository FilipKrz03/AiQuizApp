using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
	public class AlgorithmTaskConfiguration : IEntityTypeConfiguration<AlgorithmTask>
	{
		public void Configure(EntityTypeBuilder<AlgorithmTask> builder)
		{
			builder
			.Property(p => p.AdvanceNumber)
			.HasConversion(
				AdvanceNumber => AdvanceNumber.Number,
				value => AdvanceNumber.Create(value)!
				);
		}
	}
}
