using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StronglyTypedId.Data.Services;

namespace StronglyTypedId.Models

{
	public class ContractConfiguration : IEntityTypeConfiguration<Contract>
	{
		public void Configure(EntityTypeBuilder<Contract> builder)
		{
			builder.HasKey("_id", "_contractNumber");

			builder.OwnsOne(
				c => c.Key,
				ob =>
			{
				ob.WithOwner()
				.HasPrincipalKey("_id", "_contractNumber")
				.HasForeignKey(k => new { k.Id, k.ContractNumber });

				ob.Property<int>(k => k.Id)
					.HasColumnName("Id");

				ob.Property<int>(k => k.ContractNumber)
					.HasColumnName("ContractNumber");

				ob.HasKey(k => new { k.Id, k.ContractNumber });
			});

			builder.Property<int>("_id")
				.HasColumnName("Id")
				.UseIdentityColumn(1, 1);

			builder.Property<int>("_contractNumber")
				.HasColumnName("ContractNumber")
				.ValueGeneratedNever()
				.HasValueGenerator<ContractNumberGenerator>();

			builder.HasIndex("_contractNumber", nameof(Contract.Branch))
				.IsUnique(false);

			builder.HasIndex("_id")
				.IsUnique(true)
				.HasDatabaseName($"INDEX IX_Contracts_ContractNumber_{nameof(ContractBranch.Master)}")
				.HasFilter($"[Branch] = '{nameof(ContractBranch.Master)}'");

			builder.HasIndex("_id")
				.IsUnique(true)
				.HasDatabaseName($"INDEX IX_Contracts_ContractNumber_{nameof(ContractBranch.Revision)}")
				.HasFilter($"[Branch] = '{nameof(ContractBranch.Revision)}'");

			builder.HasIndex("_id")
				.IsUnique(true)
				.HasDatabaseName($"INDEX IX_Contracts_ContractNumber_{nameof(ContractBranch.UnderRevision)}")
				.HasFilter($"[Branch] = '{nameof(ContractBranch.UnderRevision)}'");



			builder.Property(c => c.LastModified)
				.ValueGeneratedNever();

			builder.Property(c => c.Branch)
				.IsRequired()
				.HasConversion<string>();

			builder.Property(c => c.Amount)
				.IsRequired()
				.HasColumnType("decimal(18,2)");
			builder.Property(c => c.SignatureDate)
				.IsRequired();
			builder.Property(c => c.ProductType)
				.IsRequired();
			builder.Property(c => c.DueDate)
				.IsRequired();

			builder.Property<int?>("_sourceContractId")
				.HasColumnName("SourceContractId")
				.UsePropertyAccessMode(PropertyAccessMode.Field);

			builder.Property<int?>("_branchedContractId")
				.HasColumnName("BranchedContractId")
				.UsePropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
