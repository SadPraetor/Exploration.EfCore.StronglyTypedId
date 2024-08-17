using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StronglyTypedId.Models
{
	public class ContractSubjectConfiguration : IEntityTypeConfiguration<ContractSubject>
	{
		public void Configure(EntityTypeBuilder<ContractSubject> builder)
		{
			builder.ToTable("ContractSubjects");

			builder.OwnsOne<ContractSubjectKey>(x => x.Key, navigationBuilder =>
			{
				//navigationBuilder.WithOwner(x => x.ContractSubject);
				navigationBuilder.WithOwner()
				.HasForeignKey(x => new { x.Id, x.Rank });

				navigationBuilder.Property(x => x.Id)
					.HasColumnName("Id");

				navigationBuilder.Property(x => x.Rank)
					.HasColumnName("Rank")
					.ValueGeneratedNever();

				navigationBuilder.HasKey(x => new { x.Id, x.Rank });
			});

			builder.Property(x => x.Ident)
				.ValueGeneratedNever();

			builder.Property<int>("_id")
				.HasColumnName("Id")
				.UseHiLo($"{nameof(ContractSubject)}_Id", "con");

			builder.Property<int>("_rank")
				.HasColumnName("Rank")
				.ValueGeneratedNever();

			builder.HasKey("_id", "_rank");

			builder.Property(x => x.Description)
				.HasMaxLength(4000);

			builder.HasMany(x => x.Insurances)
				.WithOne()
				.HasForeignKey("_contractSubjectId", "_contractSubjectRank")
				.HasPrincipalKey("_id", "_rank")
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}