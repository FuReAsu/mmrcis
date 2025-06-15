using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mmrcis.Migrations
{
    
    public partial class ModelRefactorToSupportPatientAndIncomeBill : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Persons_DoctorStaffID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Services_ServiceID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseBills_Persons_PatientID",
                table: "ExpenseBills");

            migrationBuilder.DropForeignKey(
                name: "FK_IncomeBills_Persons_PatientID",
                table: "IncomeBills");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Suppliers_SupplierID",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientCheckinOuts_PatientVitals_PatientVitalID",
                table: "PatientCheckinOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientCheckinOuts_Persons_PatientID",
                table: "PatientCheckinOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientDocuments_Persons_PatientID",
                table: "PatientDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabRecords_IncomeBills_IncomeBillID",
                table: "PatientLabRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Persons_PersonID",
                table: "Patients");

            migrationBuilder.AddColumn<int>(
                name: "PatientID",
                table: "PatientVitals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientID",
                table: "PatientLabRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsVoided",
                table: "IncomeBills",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReferenceNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IncomeBillID = table.Column<int>(type: "int", nullable: false),
                    ReceivedByOperatorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Payments_IncomeBills_IncomeBillID",
                        column: x => x.IncomeBillID,
                        principalTable: "IncomeBills",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Persons_ReceivedByOperatorID",
                        column: x => x.ReceivedByOperatorID,
                        principalTable: "Persons",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientVitals_PatientID",
                table: "PatientVitals",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLabRecords_PatientID",
                table: "PatientLabRecords",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_IncomeBillID",
                table: "Payments",
                column: "IncomeBillID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ReceivedByOperatorID",
                table: "Payments",
                column: "ReceivedByOperatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Persons_DoctorStaffID",
                table: "Appointments",
                column: "DoctorStaffID",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Services_ServiceID",
                table: "Appointments",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseBills_Patients_PatientID",
                table: "ExpenseBills",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeBills_Patients_PatientID",
                table: "IncomeBills",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Suppliers_SupplierID",
                table: "InventoryItems",
                column: "SupplierID",
                principalTable: "Suppliers",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCheckinOuts_PatientVitals_PatientVitalID",
                table: "PatientCheckinOuts",
                column: "PatientVitalID",
                principalTable: "PatientVitals",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCheckinOuts_Patients_PatientID",
                table: "PatientCheckinOuts",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientDocuments_Patients_PatientID",
                table: "PatientDocuments",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabRecords_IncomeBills_IncomeBillID",
                table: "PatientLabRecords",
                column: "IncomeBillID",
                principalTable: "IncomeBills",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabRecords_Patients_PatientID",
                table: "PatientLabRecords",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Persons_PersonID",
                table: "Patients",
                column: "PersonID",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVitals_Patients_PatientID",
                table: "PatientVitals",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "ID");
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Persons_DoctorStaffID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Services_ServiceID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseBills_Patients_PatientID",
                table: "ExpenseBills");

            migrationBuilder.DropForeignKey(
                name: "FK_IncomeBills_Patients_PatientID",
                table: "IncomeBills");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Suppliers_SupplierID",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientCheckinOuts_PatientVitals_PatientVitalID",
                table: "PatientCheckinOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientCheckinOuts_Patients_PatientID",
                table: "PatientCheckinOuts");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientDocuments_Patients_PatientID",
                table: "PatientDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabRecords_IncomeBills_IncomeBillID",
                table: "PatientLabRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabRecords_Patients_PatientID",
                table: "PatientLabRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Persons_PersonID",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientVitals_Patients_PatientID",
                table: "PatientVitals");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_PatientVitals_PatientID",
                table: "PatientVitals");

            migrationBuilder.DropIndex(
                name: "IX_PatientLabRecords_PatientID",
                table: "PatientLabRecords");

            migrationBuilder.DropColumn(
                name: "PatientID",
                table: "PatientVitals");

            migrationBuilder.DropColumn(
                name: "PatientID",
                table: "PatientLabRecords");

            migrationBuilder.AlterColumn<bool>(
                name: "IsVoided",
                table: "IncomeBills",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Persons_DoctorStaffID",
                table: "Appointments",
                column: "DoctorStaffID",
                principalTable: "Persons",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Services_ServiceID",
                table: "Appointments",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseBills_Persons_PatientID",
                table: "ExpenseBills",
                column: "PatientID",
                principalTable: "Persons",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeBills_Persons_PatientID",
                table: "IncomeBills",
                column: "PatientID",
                principalTable: "Persons",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Suppliers_SupplierID",
                table: "InventoryItems",
                column: "SupplierID",
                principalTable: "Suppliers",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCheckinOuts_PatientVitals_PatientVitalID",
                table: "PatientCheckinOuts",
                column: "PatientVitalID",
                principalTable: "PatientVitals",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCheckinOuts_Persons_PatientID",
                table: "PatientCheckinOuts",
                column: "PatientID",
                principalTable: "Persons",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientDocuments_Persons_PatientID",
                table: "PatientDocuments",
                column: "PatientID",
                principalTable: "Persons",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabRecords_IncomeBills_IncomeBillID",
                table: "PatientLabRecords",
                column: "IncomeBillID",
                principalTable: "IncomeBills",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Persons_PersonID",
                table: "Patients",
                column: "PersonID",
                principalTable: "Persons",
                principalColumn: "ID");
        }
    }
}
