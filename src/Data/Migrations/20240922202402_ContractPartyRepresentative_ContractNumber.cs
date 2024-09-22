using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StronglyTypedId.Data.Migrations
{
    /// <inheritdoc />
    public partial class ContractPartyRepresentative_ContractNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractPartyRepresentatives_ContractParties_ContractPartyId_ContractId",
                schema: "con",
                table: "ContractPartyRepresentatives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractPartyRepresentatives",
                schema: "con",
                table: "ContractPartyRepresentatives");

            migrationBuilder.DropIndex(
                name: "IX_ContractPartyRepresentatives_ContractPartyId_ContractId",
                schema: "con",
                table: "ContractPartyRepresentatives");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ContractParties_Id_ContractId",
                schema: "con",
                table: "ContractParties");

            migrationBuilder.AddColumn<int>(
                name: "ContractNumber",
                schema: "con",
                table: "ContractPartyRepresentatives",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractPartyRepresentatives",
                schema: "con",
                table: "ContractPartyRepresentatives",
                columns: new[] { "Id", "ContractPartyId", "ContractId", "ContractNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_ContractPartyRepresentatives_ContractPartyId_ContractId_ContractNumber",
                schema: "con",
                table: "ContractPartyRepresentatives",
                columns: new[] { "ContractPartyId", "ContractId", "ContractNumber" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContractPartyRepresentatives_ContractParties_ContractPartyId_ContractId_ContractNumber",
                schema: "con",
                table: "ContractPartyRepresentatives",
                columns: new[] { "ContractPartyId", "ContractId", "ContractNumber" },
                principalSchema: "con",
                principalTable: "ContractParties",
                principalColumns: new[] { "Id", "ContractId", "ContractNumber" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractPartyRepresentatives_ContractParties_ContractPartyId_ContractId_ContractNumber",
                schema: "con",
                table: "ContractPartyRepresentatives");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractPartyRepresentatives",
                schema: "con",
                table: "ContractPartyRepresentatives");

            migrationBuilder.DropIndex(
                name: "IX_ContractPartyRepresentatives_ContractPartyId_ContractId_ContractNumber",
                schema: "con",
                table: "ContractPartyRepresentatives");

            migrationBuilder.DropColumn(
                name: "ContractNumber",
                schema: "con",
                table: "ContractPartyRepresentatives");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractPartyRepresentatives",
                schema: "con",
                table: "ContractPartyRepresentatives",
                columns: new[] { "Id", "ContractPartyId", "ContractId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ContractParties_Id_ContractId",
                schema: "con",
                table: "ContractParties",
                columns: new[] { "Id", "ContractId" });

            migrationBuilder.CreateIndex(
                name: "IX_ContractPartyRepresentatives_ContractPartyId_ContractId",
                schema: "con",
                table: "ContractPartyRepresentatives",
                columns: new[] { "ContractPartyId", "ContractId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContractPartyRepresentatives_ContractParties_ContractPartyId_ContractId",
                schema: "con",
                table: "ContractPartyRepresentatives",
                columns: new[] { "ContractPartyId", "ContractId" },
                principalSchema: "con",
                principalTable: "ContractParties",
                principalColumns: new[] { "Id", "ContractId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
