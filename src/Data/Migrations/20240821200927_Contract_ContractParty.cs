using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StronglyTypedId.Data.Migrations
{
    /// <inheritdoc />
    public partial class Contract_ContractParty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ContractParties_ContractId_ContractNumber",
                schema: "con",
                table: "ContractParties",
                columns: new[] { "ContractId", "ContractNumber" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContractParties_Contracts_ContractId_ContractNumber",
                schema: "con",
                table: "ContractParties",
                columns: new[] { "ContractId", "ContractNumber" },
                principalSchema: "con",
                principalTable: "Contracts",
                principalColumns: new[] { "Id", "ContractNumber" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractParties_Contracts_ContractId_ContractNumber",
                schema: "con",
                table: "ContractParties");

            migrationBuilder.DropIndex(
                name: "IX_ContractParties_ContractId_ContractNumber",
                schema: "con",
                table: "ContractParties");
        }
    }
}
