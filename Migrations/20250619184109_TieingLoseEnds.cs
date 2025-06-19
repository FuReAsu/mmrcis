using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mmrcis.Migrations
{
    /// <inheritdoc />
    public partial class TieingLoseEnds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomeBills_PatientCheckInOut_PatientCheckInOutID",
                table: "IncomeBills");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientCheckInOut_Appointments_AppointmentID",
                table: "PatientCheckInOut");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientCheckInOut_Patients_PatientID",
                table: "PatientCheckInOut");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisitRecord_PatientCheckInOut_PatientCheckInOutID",
                table: "PatientVisitRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisitRecord_Patients_PatientID",
                table: "PatientVisitRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisitRecord_Persons_DoctorID",
                table: "PatientVisitRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientVitals_PatientVisitRecord_PatientVisitRecordID",
                table: "PatientVitals");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientVitals_Persons_MedicalStaffID",
                table: "PatientVitals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientVitals",
                table: "PatientVitals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientVisitRecord",
                table: "PatientVisitRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientCheckInOut",
                table: "PatientCheckInOut");

            migrationBuilder.RenameTable(
                name: "PatientVitals",
                newName: "PatientVitalss");

            migrationBuilder.RenameTable(
                name: "PatientVisitRecord",
                newName: "PatientVisitRecords");

            migrationBuilder.RenameTable(
                name: "PatientCheckInOut",
                newName: "PatientCheckInOuts");

            migrationBuilder.RenameIndex(
                name: "IX_PatientVitals_PatientVisitRecordID",
                table: "PatientVitalss",
                newName: "IX_PatientVitalss_PatientVisitRecordID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientVitals_MedicalStaffID",
                table: "PatientVitalss",
                newName: "IX_PatientVitalss_MedicalStaffID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientVisitRecord_PatientID",
                table: "PatientVisitRecords",
                newName: "IX_PatientVisitRecords_PatientID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientVisitRecord_PatientCheckInOutID",
                table: "PatientVisitRecords",
                newName: "IX_PatientVisitRecords_PatientCheckInOutID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientVisitRecord_DoctorID",
                table: "PatientVisitRecords",
                newName: "IX_PatientVisitRecords_DoctorID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientCheckInOut_PatientID",
                table: "PatientCheckInOuts",
                newName: "IX_PatientCheckInOuts_PatientID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientCheckInOut_AppointmentID",
                table: "PatientCheckInOuts",
                newName: "IX_PatientCheckInOuts_AppointmentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientVitalss",
                table: "PatientVitalss",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientVisitRecords",
                table: "PatientVisitRecords",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientCheckInOuts",
                table: "PatientCheckInOuts",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeBills_PatientCheckInOuts_PatientCheckInOutID",
                table: "IncomeBills",
                column: "PatientCheckInOutID",
                principalTable: "PatientCheckInOuts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCheckInOuts_Appointments_AppointmentID",
                table: "PatientCheckInOuts",
                column: "AppointmentID",
                principalTable: "Appointments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCheckInOuts_Patients_PatientID",
                table: "PatientCheckInOuts",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVisitRecords_PatientCheckInOuts_PatientCheckInOutID",
                table: "PatientVisitRecords",
                column: "PatientCheckInOutID",
                principalTable: "PatientCheckInOuts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVisitRecords_Patients_PatientID",
                table: "PatientVisitRecords",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVisitRecords_Persons_DoctorID",
                table: "PatientVisitRecords",
                column: "DoctorID",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVitalss_PatientVisitRecords_PatientVisitRecordID",
                table: "PatientVitalss",
                column: "PatientVisitRecordID",
                principalTable: "PatientVisitRecords",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVitalss_Persons_MedicalStaffID",
                table: "PatientVitalss",
                column: "MedicalStaffID",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomeBills_PatientCheckInOuts_PatientCheckInOutID",
                table: "IncomeBills");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientCheckInOuts_Appointments_AppointmentID",
                table: "PatientCheckInOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientCheckInOuts_Patients_PatientID",
                table: "PatientCheckInOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisitRecords_PatientCheckInOuts_PatientCheckInOutID",
                table: "PatientVisitRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisitRecords_Patients_PatientID",
                table: "PatientVisitRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisitRecords_Persons_DoctorID",
                table: "PatientVisitRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientVitalss_PatientVisitRecords_PatientVisitRecordID",
                table: "PatientVitalss");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientVitalss_Persons_MedicalStaffID",
                table: "PatientVitalss");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientVitalss",
                table: "PatientVitalss");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientVisitRecords",
                table: "PatientVisitRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientCheckInOuts",
                table: "PatientCheckInOuts");

            migrationBuilder.RenameTable(
                name: "PatientVitalss",
                newName: "PatientVitals");

            migrationBuilder.RenameTable(
                name: "PatientVisitRecords",
                newName: "PatientVisitRecord");

            migrationBuilder.RenameTable(
                name: "PatientCheckInOuts",
                newName: "PatientCheckInOut");

            migrationBuilder.RenameIndex(
                name: "IX_PatientVitalss_PatientVisitRecordID",
                table: "PatientVitals",
                newName: "IX_PatientVitals_PatientVisitRecordID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientVitalss_MedicalStaffID",
                table: "PatientVitals",
                newName: "IX_PatientVitals_MedicalStaffID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientVisitRecords_PatientID",
                table: "PatientVisitRecord",
                newName: "IX_PatientVisitRecord_PatientID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientVisitRecords_PatientCheckInOutID",
                table: "PatientVisitRecord",
                newName: "IX_PatientVisitRecord_PatientCheckInOutID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientVisitRecords_DoctorID",
                table: "PatientVisitRecord",
                newName: "IX_PatientVisitRecord_DoctorID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientCheckInOuts_PatientID",
                table: "PatientCheckInOut",
                newName: "IX_PatientCheckInOut_PatientID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientCheckInOuts_AppointmentID",
                table: "PatientCheckInOut",
                newName: "IX_PatientCheckInOut_AppointmentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientVitals",
                table: "PatientVitals",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientVisitRecord",
                table: "PatientVisitRecord",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientCheckInOut",
                table: "PatientCheckInOut",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeBills_PatientCheckInOut_PatientCheckInOutID",
                table: "IncomeBills",
                column: "PatientCheckInOutID",
                principalTable: "PatientCheckInOut",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCheckInOut_Appointments_AppointmentID",
                table: "PatientCheckInOut",
                column: "AppointmentID",
                principalTable: "Appointments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCheckInOut_Patients_PatientID",
                table: "PatientCheckInOut",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVisitRecord_PatientCheckInOut_PatientCheckInOutID",
                table: "PatientVisitRecord",
                column: "PatientCheckInOutID",
                principalTable: "PatientCheckInOut",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVisitRecord_Patients_PatientID",
                table: "PatientVisitRecord",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVisitRecord_Persons_DoctorID",
                table: "PatientVisitRecord",
                column: "DoctorID",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVitals_PatientVisitRecord_PatientVisitRecordID",
                table: "PatientVitals",
                column: "PatientVisitRecordID",
                principalTable: "PatientVisitRecord",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVitals_Persons_MedicalStaffID",
                table: "PatientVitals",
                column: "MedicalStaffID",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
