using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mmrcis.Migrations
{
    /// <inheritdoc />
    public partial class AddedPatientRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientCheckInOutID",
                table: "IncomeBills",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PatientCheckInOut",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientCheckInOut", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientCheckInOut_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientVisitRecord",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    PatientCheckInOutID = table.Column<int>(type: "int", nullable: false),
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    DateOfVisit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Diagnoses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prescriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientVisitRecord", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientVisitRecord_PatientCheckInOut_PatientCheckInOutID",
                        column: x => x.PatientCheckInOutID,
                        principalTable: "PatientCheckInOut",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientVisitRecord_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientVisitRecord_Persons_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Persons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientVitals",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientVisitRecordID = table.Column<int>(type: "int", nullable: false),
                    MedicalStaffID = table.Column<int>(type: "int", nullable: false),
                    Temperature = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    PulseRate = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    RespiratorRate = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    BloodPressureSystolic = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    BloodPressureDiastolic = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    OxygenSaturation = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientVitals", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientVitals_PatientVisitRecord_PatientVisitRecordID",
                        column: x => x.PatientVisitRecordID,
                        principalTable: "PatientVisitRecord",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientVitals_Persons_MedicalStaffID",
                        column: x => x.MedicalStaffID,
                        principalTable: "Persons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncomeBills_PatientCheckInOutID",
                table: "IncomeBills",
                column: "PatientCheckInOutID",
                unique: true,
                filter: "[PatientCheckInOutID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PatientCheckInOut_PatientID",
                table: "PatientCheckInOut",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitRecord_DoctorID",
                table: "PatientVisitRecord",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitRecord_PatientCheckInOutID",
                table: "PatientVisitRecord",
                column: "PatientCheckInOutID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitRecord_PatientID",
                table: "PatientVisitRecord",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVitals_MedicalStaffID",
                table: "PatientVitals",
                column: "MedicalStaffID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVitals_PatientVisitRecordID",
                table: "PatientVitals",
                column: "PatientVisitRecordID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeBills_PatientCheckInOut_PatientCheckInOutID",
                table: "IncomeBills",
                column: "PatientCheckInOutID",
                principalTable: "PatientCheckInOut",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomeBills_PatientCheckInOut_PatientCheckInOutID",
                table: "IncomeBills");

            migrationBuilder.DropTable(
                name: "PatientVitals");

            migrationBuilder.DropTable(
                name: "PatientVisitRecord");

            migrationBuilder.DropTable(
                name: "PatientCheckInOut");

            migrationBuilder.DropIndex(
                name: "IX_IncomeBills_PatientCheckInOutID",
                table: "IncomeBills");

            migrationBuilder.DropColumn(
                name: "PatientCheckInOutID",
                table: "IncomeBills");
        }
    }
}
