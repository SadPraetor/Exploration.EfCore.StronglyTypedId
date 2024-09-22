﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StronglyTypedId.Data;

#nullable disable

namespace StronglyTypedId.Data.Migrations
{
    [DbContext(typeof(ContractContext))]
    [Migration("20240922202402_ContractPartyRepresentative_ContractNumber")]
    partial class ContractPartyRepresentative_ContractNumber
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("con")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.HasSequence<int>("Contract_Number_Bond", "con")
                .StartsAt(40000000L)
                .HasMax(69999999L);

            modelBuilder.HasSequence<int>("Contract_Number_Insurance", "con")
                .StartsAt(90000000L)
                .HasMax(99999999L);

            modelBuilder.HasSequence<int>("Contract_Number_Loan", "con")
                .StartsAt(70000000L)
                .HasMax(89999999L);

            modelBuilder.HasSequence<int>("Contract_Number_Stock", "con")
                .StartsAt(10000000L)
                .HasMax(39999999L);

            modelBuilder.HasSequence<int>("ContractParty_Id", "con")
                .HasMax(99999999L);

            modelBuilder.HasSequence<int>("ContractPartyRepresentative_Id", "con")
                .HasMax(99999999L);

            modelBuilder.HasSequence<int>("ContractSubject_Id", "con")
                .HasMax(99999999L);

            modelBuilder.HasSequence<int>("Insurance_Id", "con")
                .HasMax(99999999L);

            modelBuilder.Entity("StronglyTypedId.Models.Contract", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("_id"));

                    b.Property<int>("_contractNumber")
                        .HasColumnType("int")
                        .HasColumnName("ContractNumber");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Branch")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductType")
                        .HasColumnType("int");

                    b.Property<DateTime>("SignatureDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int?>("_branchedContractId")
                        .HasColumnType("int")
                        .HasColumnName("BranchedContractId");

                    b.Property<int?>("_sourceContractId")
                        .HasColumnType("int")
                        .HasColumnName("SourceContractId");

                    b.HasKey("_id", "_contractNumber");

                    b.HasIndex("_id")
                        .IsUnique()
                        .HasDatabaseName("INDEX IX_Contracts_ContractNumber_UnderRevision")
                        .HasFilter("[Branch] = 'UnderRevision'");

                    b.HasIndex("_contractNumber", "Branch");

                    b.ToTable("Contracts", "con");
                });

            modelBuilder.Entity("StronglyTypedId.Models.ContractParty", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseHiLo(b.Property<int>("_id"), "ContractParty_Id", "con");

                    b.Property<int>("_contractId")
                        .HasColumnType("int")
                        .HasColumnName("ContractId");

                    b.Property<int>("_contractNumber")
                        .HasColumnType("int")
                        .HasColumnName("ContractNumber");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("_id", "_contractId", "_contractNumber");

                    b.HasIndex("_contractId", "_contractNumber");

                    b.ToTable("ContractParties", "con");
                });

            modelBuilder.Entity("StronglyTypedId.Models.ContractPartyRepresentative", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseHiLo(b.Property<int>("_id"), "ContractPartyRepresentative_Id", "con");

                    b.Property<int>("_contractPartyId")
                        .HasColumnType("int")
                        .HasColumnName("ContractPartyId");

                    b.Property<int>("_contractId")
                        .HasColumnType("int")
                        .HasColumnName("ContractId");

                    b.Property<int>("_contractNumber")
                        .HasColumnType("int")
                        .HasColumnName("ContractNumber");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("_id", "_contractPartyId", "_contractId", "_contractNumber");

                    b.HasIndex("_contractPartyId", "_contractId", "_contractNumber");

                    b.ToTable("ContractPartyRepresentatives", "con");
                });

            modelBuilder.Entity("StronglyTypedId.Models.ContractSubject", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseHiLo(b.Property<int>("_id"), "ContractSubject_Id", "con");

                    b.Property<int>("_rank")
                        .HasColumnType("int")
                        .HasColumnName("Rank");

                    b.Property<string>("Description")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<int>("Ident")
                        .HasColumnType("int");

                    b.HasKey("_id", "_rank");

                    b.ToTable("ContractSubjects", "con");
                });

            modelBuilder.Entity("StronglyTypedId.Models.Insurance", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseHiLo(b.Property<int>("_id"), "Insurance_Id", "con");

                    b.Property<int>("_contractSubjectId")
                        .HasColumnType("int")
                        .HasColumnName("ContractSubject_Id");

                    b.Property<int>("_contractSubjectRank")
                        .HasColumnType("int")
                        .HasColumnName("ContractSubject_Rank");

                    b.Property<DateTime?>("SignedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ValidFrom")
                        .HasColumnType("datetime2");

                    b.HasKey("_id", "_contractSubjectId", "_contractSubjectRank");

                    b.HasIndex("_contractSubjectId", "_contractSubjectRank");

                    b.ToTable("Insurances", "con");
                });

            modelBuilder.Entity("StronglyTypedId.Models.Contract", b =>
                {
                    b.OwnsOne("StronglyTypedId.Models.ContractKey", "Key", b1 =>
                        {
                            b1.Property<int>("ContractId")
                                .HasColumnType("int")
                                .HasColumnName("Id");

                            b1.Property<int>("ContractNumber")
                                .HasColumnType("int")
                                .HasColumnName("ContractNumber");

                            b1.HasKey("ContractId", "ContractNumber");

                            b1.ToTable("Contracts", "con");

                            b1.WithOwner()
                                .HasForeignKey("ContractId", "ContractNumber");
                        });

                    b.Navigation("Key")
                        .IsRequired();
                });

            modelBuilder.Entity("StronglyTypedId.Models.ContractParty", b =>
                {
                    b.HasOne("StronglyTypedId.Models.Contract", "Contract")
                        .WithMany("ContractParties")
                        .HasForeignKey("_contractId", "_contractNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("StronglyTypedId.Models.ContractPartyKey", "Key", b1 =>
                        {
                            b1.Property<int>("ContractPartyId")
                                .HasColumnType("int")
                                .HasColumnName("Id");

                            b1.Property<int>("ContractId")
                                .HasColumnType("int")
                                .HasColumnName("ContractId");

                            b1.Property<int>("ContractNumber")
                                .HasColumnType("int")
                                .HasColumnName("ContractNumber");

                            b1.HasKey("ContractPartyId", "ContractId", "ContractNumber");

                            b1.ToTable("ContractParties", "con");

                            b1.WithOwner()
                                .HasForeignKey("ContractPartyId", "ContractId", "ContractNumber");
                        });

                    b.Navigation("Contract");

                    b.Navigation("Key")
                        .IsRequired();
                });

            modelBuilder.Entity("StronglyTypedId.Models.ContractPartyRepresentative", b =>
                {
                    b.HasOne("StronglyTypedId.Models.ContractParty", null)
                        .WithMany("Representatives")
                        .HasForeignKey("_contractPartyId", "_contractId", "_contractNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("StronglyTypedId.Models.ContractPartyRepresentativeKey", "Key", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasColumnName("Id");

                            SqlServerPropertyBuilderExtensions.UseHiLo(b1.Property<int>("Id"), "ContractPartyRepresentative_Id", "con");

                            b1.Property<int>("ContractPartyId")
                                .HasColumnType("int")
                                .HasColumnName("ContractPartyId");

                            b1.Property<int>("ContractId")
                                .HasColumnType("int")
                                .HasColumnName("ContractId");

                            b1.Property<int>("ContractNumber")
                                .HasColumnType("int")
                                .HasColumnName("ContractNumber");

                            b1.HasKey("Id", "ContractPartyId", "ContractId", "ContractNumber");

                            b1.ToTable("ContractPartyRepresentatives", "con");

                            b1.WithOwner()
                                .HasForeignKey("Id", "ContractPartyId", "ContractId", "ContractNumber");
                        });

                    b.Navigation("Key")
                        .IsRequired();
                });

            modelBuilder.Entity("StronglyTypedId.Models.ContractSubject", b =>
                {
                    b.OwnsOne("StronglyTypedId.Models.ContractSubjectKey", "Key", b1 =>
                        {
                            b1.Property<int>("Id")
                                .HasColumnType("int")
                                .HasColumnName("Id");

                            b1.Property<int>("Rank")
                                .HasColumnType("int")
                                .HasColumnName("Rank");

                            b1.HasKey("Id", "Rank");

                            b1.ToTable("ContractSubjects", "con");

                            b1.WithOwner()
                                .HasForeignKey("Id", "Rank");
                        });

                    b.Navigation("Key")
                        .IsRequired();
                });

            modelBuilder.Entity("StronglyTypedId.Models.Insurance", b =>
                {
                    b.HasOne("StronglyTypedId.Models.ContractSubject", null)
                        .WithMany("Insurances")
                        .HasForeignKey("_contractSubjectId", "_contractSubjectRank")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("StronglyTypedId.Models.InsuranceKey", "Key", b1 =>
                        {
                            b1.Property<int>("Id")
                                .HasColumnType("int")
                                .HasColumnName("Id");

                            b1.Property<int>("_contractSubjectId")
                                .HasColumnType("int")
                                .HasColumnName("ContractSubject_Id");

                            b1.Property<int>("_contractSubjectRank")
                                .HasColumnType("int")
                                .HasColumnName("ContractSubject_Rank");

                            b1.HasKey("Id", "_contractSubjectId", "_contractSubjectRank");

                            b1.ToTable("Insurances", "con");

                            b1.WithOwner()
                                .HasForeignKey("Id", "_contractSubjectId", "_contractSubjectRank");

                            b1.OwnsOne("StronglyTypedId.Models.InsuranceKey+ContractSubjectKeyInsurance", "ContractSubjectKey", b2 =>
                                {
                                    b2.Property<int>("_insuranceKeyId")
                                        .HasColumnType("int");

                                    b2.Property<int>("Id")
                                        .HasColumnType("int")
                                        .HasColumnName("ContractSubject_Id");

                                    b2.Property<int>("Rank")
                                        .HasColumnType("int")
                                        .HasColumnName("ContractSubject_Rank");

                                    b2.HasKey("_insuranceKeyId", "Id", "Rank");

                                    b2.ToTable("Insurances", "con");

                                    b2.WithOwner()
                                        .HasForeignKey("_insuranceKeyId", "Id", "Rank");
                                });

                            b1.Navigation("ContractSubjectKey")
                                .IsRequired();
                        });

                    b.Navigation("Key")
                        .IsRequired();
                });

            modelBuilder.Entity("StronglyTypedId.Models.Contract", b =>
                {
                    b.Navigation("ContractParties");
                });

            modelBuilder.Entity("StronglyTypedId.Models.ContractParty", b =>
                {
                    b.Navigation("Representatives");
                });

            modelBuilder.Entity("StronglyTypedId.Models.ContractSubject", b =>
                {
                    b.Navigation("Insurances");
                });
#pragma warning restore 612, 618
        }
    }
}
