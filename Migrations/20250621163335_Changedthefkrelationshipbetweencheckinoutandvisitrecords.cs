using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mmrcis.Migrations
{
    /// <inheritdoc />
    public partial class Changedthefkrelationshipbetweencheckinoutandvisitrecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisitRecords_PatientCheckInOuts_PatientCheckInOutID",
                table: "PatientVisitRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVisitRecords_PatientCheckInOuts_PatientCheckInOutID",
                table: "PatientVisitRecords",
                column: "PatientCheckInOutID",
                principalTable: "PatientCheckInOuts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisitRecords_PatientCheckInOuts_PatientCheckInOutID",
                table: "PatientVisitRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVisitRecords_PatientCheckInOuts_PatientCheckInOutID",
                table: "PatientVisitRecords",
                column: "PatientCheckInOutID",
                principalTable: "PatientCheckInOuts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
