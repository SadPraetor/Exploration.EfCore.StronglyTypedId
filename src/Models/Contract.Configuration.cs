using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StronglyTypedId.Models

{
	public class ContractConfiguration : IEntityTypeConfiguration<Contract>
	{
		public void Configure(EntityTypeBuilder<Contract> builder)
		{
			//builder.ToTable(nameof(ContractContext.Contracts), tb => tb.HasTrigger("trg_UpdateContractNumberAfterInsert"));

			builder.HasKey(c => new { c.Id, c.ContractNumber });

			builder.Property(c => c.Id)
				.UseIdentityColumn(1, 1);

			builder.HasIndex(c => new { c.ContractNumber, c.Branch })
				.IsUnique(false);

			builder.HasIndex(c => c.Id)
				.IsUnique(true)
				.HasDatabaseName($"INDEX IX_Contracts_ContractNumber_{nameof(ContractBranch.Master)}")
				.HasFilter($"[Branch] = '{nameof(ContractBranch.Master)}'");

			builder.HasIndex(c => c.Id)
				.IsUnique(true)
				.HasDatabaseName($"INDEX IX_Contracts_ContractNumber_{nameof(ContractBranch.Revision)}")
				.HasFilter($"[Branch] = '{nameof(ContractBranch.Revision)}'");

			builder.HasIndex(c => c.Id)
				.IsUnique(true)
				.HasDatabaseName($"INDEX IX_Contracts_ContractNumber_{nameof(ContractBranch.UnderRevision)}")
				.HasFilter($"[Branch] = '{nameof(ContractBranch.UnderRevision)}'");

			builder.Property(c => c.ContractNumber)
				//.HasDefaultValue(0)
				//.HasDefaultValueSql();	//should work both ways, has default value
				//.ValueGeneratedOnAdd()	//triggers read from database, assumption that db generates
				//.HasValueGenerator<ContractNumberGenerator>()
				.UseHiLo($"{nameof(ContractPartyRepresentative)}_Id", "con");
			;

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


			builder.HasOne(c => c.BranchedContract)
				.WithOne()
				.HasForeignKey<Contract>("_branchedContractId")
				.HasPrincipalKey<Contract>(c => c.Id)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.ClientSetNull);

			builder.HasOne(c => c.SourceContract)
				.WithOne()
				.HasForeignKey<Contract>("_sourceContractId")
				.HasPrincipalKey<Contract>(c => c.Id)
				.IsRequired(false)
				.OnDelete(DeleteBehavior.ClientSetNull);

			builder.Property<int?>("_sourceContractId")
				.HasColumnName("SourceContractId")
				.UsePropertyAccessMode(PropertyAccessMode.Field)
				;
			builder.Property<int?>("_branchedContractId")
				.HasColumnName("BranchedContractId")
				.UsePropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
