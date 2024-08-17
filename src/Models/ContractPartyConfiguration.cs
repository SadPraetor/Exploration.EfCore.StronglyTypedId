using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StronglyTypedId.Models
{
	public class ContractPartyConfiguration : IEntityTypeConfiguration<ContractParty>
	{
		public void Configure(EntityTypeBuilder<ContractParty> builder)
		{
			builder.ToTable("ContractParties");

			builder.Property(x => x.Name)
				.IsRequired()
				.HasMaxLength(500);

			builder.OwnsOne<ContractPartyKey>(
				x => x.Key,
				nb =>
				{
					nb.WithOwner()
					.HasPrincipalKey("_id", "_contractId")
					.HasForeignKey(k => new { k.Id, k.ContractId });

					nb.Property(x => x.Id)
						.HasColumnName("Id")
						.ValueGeneratedNever();

					nb.Property(x => x.ContractId)
					.HasColumnName("ContractId")
					.ValueGeneratedNever();
				}
			);

			builder.Property<int>("_id")
				.HasColumnName("Id")
				.UseHiLo($"{nameof(ContractParty)}_Id", "con");

			builder.Property<int>("_contractId")
				.HasColumnName("ContractId");
			//.ValueGeneratedNever();

			builder.HasKey("_id", "_contractId");

			builder.HasMany<ContractPartyRepresentative>(x => x.Representatives)
				.WithOne()
				.HasForeignKey("_contractPartyId", "_contractId")
				.HasPrincipalKey("_id", "_contractId")
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}