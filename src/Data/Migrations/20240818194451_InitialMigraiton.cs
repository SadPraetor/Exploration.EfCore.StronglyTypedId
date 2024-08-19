using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StronglyTypedId.Data.Migrations
{
	/// <inheritdoc />
	public partial class InitialMigraiton : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "con");

			migrationBuilder.CreateSequence<int>(
				name: "Contract_Number_Bond",
				schema: "con",
				startValue: 40000000L,
				maxValue: 69999999L);

			migrationBuilder.CreateSequence<int>(
				name: "Contract_Number_Insurance",
				schema: "con",
				startValue: 90000000L,
				maxValue: 99999999L);

			migrationBuilder.CreateSequence<int>(
				name: "Contract_Number_Loan",
				schema: "con",
				startValue: 70000000L,
				maxValue: 89999999L);

			migrationBuilder.CreateSequence<int>(
				name: "Contract_Number_Stock",
				schema: "con",
				startValue: 10000000L,
				maxValue: 39999999L);

			migrationBuilder.CreateSequence<int>(
				name: "ContractParty_Id",
				schema: "con",
				maxValue: 99999999L);

			migrationBuilder.CreateSequence<int>(
				name: "ContractPartyRepresentative_Id",
				schema: "con",
				maxValue: 99999999L);

			migrationBuilder.CreateSequence<int>(
				name: "ContractSubject_Id",
				schema: "con",
				maxValue: 99999999L);

			migrationBuilder.CreateSequence<int>(
				name: "Insurance_Id",
				schema: "con",
				maxValue: 99999999L);

			migrationBuilder.CreateTable(
				name: "ContractParties",
				schema: "con",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false),
					ContractId = table.Column<int>(type: "int", nullable: false),
					Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ContractParties", x => new { x.Id, x.ContractId });
				});

			migrationBuilder.CreateTable(
				name: "Contracts",
				schema: "con",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ContractNumber = table.Column<int>(type: "int", nullable: false),
					Branch = table.Column<string>(type: "nvarchar(450)", nullable: false),
					State = table.Column<int>(type: "int", nullable: false),
					Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
					SignatureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					ProductType = table.Column<int>(type: "int", nullable: false),
					DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
					BranchedContractId = table.Column<int>(type: "int", nullable: true),
					SourceContractId = table.Column<int>(type: "int", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Contracts", x => new { x.Id, x.ContractNumber });
				});

			migrationBuilder.CreateTable(
				name: "ContractSubjects",
				schema: "con",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false),
					Rank = table.Column<int>(type: "int", nullable: false),
					Ident = table.Column<int>(type: "int", nullable: false),
					Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ContractSubjects", x => new { x.Id, x.Rank });
				});

			migrationBuilder.CreateTable(
				name: "ContractPartyRepresentatives",
				schema: "con",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false),
					ContractPartyId = table.Column<int>(type: "int", nullable: false),
					ContractId = table.Column<int>(type: "int", nullable: false),
					Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ContractPartyRepresentatives", x => new { x.Id, x.ContractPartyId, x.ContractId });
					table.ForeignKey(
						name: "FK_ContractPartyRepresentatives_ContractParties_ContractPartyId_ContractId",
						columns: x => new { x.ContractPartyId, x.ContractId },
						principalSchema: "con",
						principalTable: "ContractParties",
						principalColumns: new[] { "Id", "ContractId" },
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Insurances",
				schema: "con",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false),
					ContractSubject_Id = table.Column<int>(type: "int", nullable: false),
					ContractSubject_Rank = table.Column<int>(type: "int", nullable: false),
					SignedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
					ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Insurances", x => new { x.Id, x.ContractSubject_Id, x.ContractSubject_Rank });
					table.ForeignKey(
						name: "FK_Insurances_ContractSubjects_ContractSubject_Id_ContractSubject_Rank",
						columns: x => new { x.ContractSubject_Id, x.ContractSubject_Rank },
						principalSchema: "con",
						principalTable: "ContractSubjects",
						principalColumns: new[] { "Id", "Rank" },
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_ContractPartyRepresentatives_ContractPartyId_ContractId",
				schema: "con",
				table: "ContractPartyRepresentatives",
				columns: new[] { "ContractPartyId", "ContractId" });

			migrationBuilder.CreateIndex(
				name: "INDEX IX_Contracts_ContractNumber_UnderRevision",
				schema: "con",
				table: "Contracts",
				column: "Id",
				unique: true,
				filter: "[Branch] = 'UnderRevision'");

			migrationBuilder.CreateIndex(
				name: "INDEX IX_Contracts_ContractNumber_Master",
				schema: "con",
				table: "Contracts",
				column: "Id",
				unique: true,
				filter: "[Branch] = 'Master'");

			migrationBuilder.CreateIndex(
				name: "INDEX IX_Contracts_ContractNumber_Revision",
				schema: "con",
				table: "Contracts",
				column: "Id",
				unique: true,
				filter: "[Branch] = 'Revision'");

			migrationBuilder.CreateIndex(
				name: "IX_Contracts_ContractNumber_Branch",
				schema: "con",
				table: "Contracts",
				columns: new[] { "ContractNumber", "Branch" });

			migrationBuilder.CreateIndex(
				name: "IX_Insurances_ContractSubject_Id_ContractSubject_Rank",
				schema: "con",
				table: "Insurances",
				columns: new[] { "ContractSubject_Id", "ContractSubject_Rank" });
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ContractPartyRepresentatives",
				schema: "con");

			migrationBuilder.DropTable(
				name: "Contracts",
				schema: "con");

			migrationBuilder.DropTable(
				name: "Insurances",
				schema: "con");

			migrationBuilder.DropTable(
				name: "ContractParties",
				schema: "con");

			migrationBuilder.DropTable(
				name: "ContractSubjects",
				schema: "con");

			migrationBuilder.DropSequence(
				name: "Contract_Number_Bond",
				schema: "con");

			migrationBuilder.DropSequence(
				name: "Contract_Number_Insurance",
				schema: "con");

			migrationBuilder.DropSequence(
				name: "Contract_Number_Loan",
				schema: "con");

			migrationBuilder.DropSequence(
				name: "Contract_Number_Stock",
				schema: "con");

			migrationBuilder.DropSequence(
				name: "ContractParty_Id",
				schema: "con");

			migrationBuilder.DropSequence(
				name: "ContractPartyRepresentative_Id",
				schema: "con");

			migrationBuilder.DropSequence(
				name: "ContractSubject_Id",
				schema: "con");

			migrationBuilder.DropSequence(
				name: "Insurance_Id",
				schema: "con");
		}
	}
}
