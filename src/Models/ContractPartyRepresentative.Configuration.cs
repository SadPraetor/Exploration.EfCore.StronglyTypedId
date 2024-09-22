using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StronglyTypedId.Models
{
	public class ContractPartyRepresentativeConfiguration : IEntityTypeConfiguration<ContractPartyRepresentative>
	{
		public void Configure(EntityTypeBuilder<ContractPartyRepresentative> builder)
		{
			builder.ToTable("ContractPartyRepresentatives");

			builder.Property(x => x.Name)
				.IsRequired()
				.HasMaxLength(500);

			builder.OwnsOne<ContractPartyRepresentativeKey>(
				x => x.Key,
				nb =>
				{
					nb.WithOwner()
					.HasPrincipalKey("_id", "_contractPartyId", "_contractId", "_contractNumber") //causes actually duplication of columns in generated migration
					.HasForeignKey(k => new { k.Id, k.ContractPartyId, k.ContractId, k.ContractNumber });

					nb.Property(x => x.Id)
						.HasColumnName("Id")
						.UseHiLo($"{nameof(ContractPartyRepresentative)}_Id", "con");

					nb.Property(x => x.ContractPartyId)
					.HasColumnName("ContractPartyId")
					.ValueGeneratedNever();

					nb.Property(x => x.ContractId)
					.HasColumnName("ContractId")
					.ValueGeneratedNever();

					nb.Property(x => x.ContractNumber)
					.HasColumnName("ContractNumber")
					.ValueGeneratedNever();

					nb.HasKey(x => new { x.Id, x.ContractPartyId, x.ContractId, x.ContractNumber });
				}
			);

			builder.Property<int>("_id")
				.HasColumnName("Id")
				.UseHiLo($"{nameof(ContractPartyRepresentative)}_Id", "con");


			builder.Property<int>("_contractPartyId")
				.HasColumnName("ContractPartyId");


			builder.Property<int>("_contractId")
				.HasColumnName("ContractId");

			builder.Property<int>("_contractNumber")
				.HasColumnName("ContractNumber");

			builder.HasKey("_id", "_contractPartyId", "_contractId", "_contractNumber");
		}
	}
}
