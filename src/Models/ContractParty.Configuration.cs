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
						.HasPrincipalKey("_id", "_contractId", "_contractNumber")
						.HasForeignKey(cp => new { cp.ContractPartyId, cp.ContractId, cp.ContractNumber });

					nb.Property(k => k.ContractPartyId)
						.HasColumnName("Id")
						.ValueGeneratedNever();

					nb.Property(k => k.ContractId)
						.HasColumnName("ContractId")
						.ValueGeneratedNever();

					nb.Property(k => k.ContractNumber)
						.HasColumnName("ContractNumber")
						.ValueGeneratedNever();

					nb.HasKey(k => new { k.ContractPartyId, k.ContractId, k.ContractNumber });
				}
			);

			builder.Property<int>("_id")
				.HasColumnName("Id")
				.UseHiLo($"{nameof(ContractParty)}_Id", "con");

			builder.Property<int>("_contractId")
				.HasColumnName("ContractId");

			builder.Property<int>("_contractNumber")
				.HasColumnName("ContractNumber");


			builder.HasKey("_id", "_contractId", "_contractNumber");

			builder.HasMany<ContractPartyRepresentative>(x => x.Representatives)
				.WithOne()
				.HasForeignKey("_contractPartyId", "_contractId")
				.HasPrincipalKey("_id", "_contractId")
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}