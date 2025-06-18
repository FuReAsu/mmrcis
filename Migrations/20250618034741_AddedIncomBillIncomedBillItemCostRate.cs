using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mmrcis.Migrations
{
    /// <inheritdoc />
    public partial class AddedIncomBillIncomedBillItemCostRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostRates",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostRates", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "IncomeBills",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PersonID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeBills", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IncomeBills_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeBills_Persons_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Persons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeBillItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostRateID = table.Column<int>(type: "int", nullable: false),
                    IncomeBillID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeBillItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IncomeBillItems_CostRates_CostRateID",
                        column: x => x.CostRateID,
                        principalTable: "CostRates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncomeBillItems_IncomeBills_IncomeBillID",
                        column: x => x.IncomeBillID,
                        principalTable: "IncomeBills",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomeBillItems_CostRateID",
                table: "IncomeBillItems",
                column: "CostRateID");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeBillItems_IncomeBillID",
                table: "IncomeBillItems",
                column: "IncomeBillID");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeBills_PatientID",
                table: "IncomeBills",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeBills_PersonID",
                table: "IncomeBills",
                column: "PersonID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncomeBillItems");

            migrationBuilder.DropTable(
                name: "CostRates");

            migrationBuilder.DropTable(
                name: "IncomeBills");
        }
    }
}
