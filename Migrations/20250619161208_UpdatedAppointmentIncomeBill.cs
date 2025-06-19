using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mmrcis.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAppointmentIncomeBill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentID",
                table: "PatientCheckInOut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "BillTotal",
                table: "IncomeBills",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_PatientCheckInOut_AppointmentID",
                table: "PatientCheckInOut",
                column: "AppointmentID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCheckInOut_Appointments_AppointmentID",
                table: "PatientCheckInOut",
                column: "AppointmentID",
                principalTable: "Appointments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientCheckInOut_Appointments_AppointmentID",
                table: "PatientCheckInOut");

            migrationBuilder.DropIndex(
                name: "IX_PatientCheckInOut_AppointmentID",
                table: "PatientCheckInOut");

            migrationBuilder.DropColumn(
                name: "AppointmentID",
                table: "PatientCheckInOut");

            migrationBuilder.DropColumn(
                name: "BillTotal",
                table: "IncomeBills");
        }
    }
}
