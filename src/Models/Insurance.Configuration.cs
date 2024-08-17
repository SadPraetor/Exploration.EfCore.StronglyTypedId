using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StronglyTypedId.Models
{
	public class InsuranceConfiguration : IEntityTypeConfiguration<Insurance>
	{
		public void Configure(EntityTypeBuilder<Insurance> builder)
		{
			builder.ToTable("Insurances");

			builder.OwnsOne<InsuranceKey>(x => x.Key, nb =>
			{
				nb.WithOwner()
				.HasPrincipalKey("_id", "_contractSubjectId", "_contractSubjectRank")
				.HasForeignKey("Id", "_contractSubjectId", "_contractSubjectRank");

				nb.Property(x => x.Id)
					.HasColumnName("Id");


				//this part is required in order to be able to use nb.HasKey
				nb.Property<int>("_contractSubjectId")
					.HasColumnName("ContractSubject_Id")
					.ValueGeneratedNever();
				nb.Property<int>("_contractSubjectRank")
					.HasColumnName("ContractSubject_Rank")
					.ValueGeneratedNever();


				//!!! this is actually important
				//without this, it will generate value for _id in owner, but not fill Id in key
				//on save this discrepancy causes error                
				nb.HasKey("Id", "_contractSubjectId", "_contractSubjectRank");

				nb.OwnsOne<InsuranceKey.ContractSubjectKeyInsurance>(
					x => x.ContractSubjectKey,
					cnb =>
					{
						cnb.WithOwner()
						.HasPrincipalKey("Id", "_contractSubjectId", "_contractSubjectRank")
						.HasForeignKey("_insuranceKeyId", "Id", "Rank");

						cnb.Property<int>("_insuranceKeyId");

						cnb.Property(x => x.Id)
						.HasColumnName("ContractSubject_Id");

						cnb.Property(x => x.Rank)
						.HasColumnName("ContractSubject_Rank");

						cnb.HasKey("_insuranceKeyId", "Id", "Rank");
					}
				);
			});

			builder.Property<int>("_id")
				.HasColumnName("Id")
				.UseHiLo($"{nameof(Insurance)}_Id", "con");

			builder.Property<int>("_contractSubjectId")
				.HasColumnName("ContractSubject_Id")
				.ValueGeneratedNever();
			builder.Property<int>("_contractSubjectRank")
				.HasColumnName("ContractSubject_Rank")
				.ValueGeneratedNever();

			builder.HasKey("_id", "_contractSubjectId", "_contractSubjectRank");
		}
	}
}