using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mmrcis.Migrations
{
    
    public partial class AddedOperatorToPatientLabRecord : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OperatorID",
                table: "PatientLabRecords",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientLabRecords_OperatorID",
                table: "PatientLabRecords",
                column: "OperatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabRecords_Persons_OperatorID",
                table: "PatientLabRecords",
                column: "OperatorID",
                principalTable: "Persons",
                principalColumn: "ID");
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabRecords_Persons_OperatorID",
                table: "PatientLabRecords");

            migrationBuilder.DropIndex(
                name: "IX_PatientLabRecords_OperatorID",
                table: "PatientLabRecords");

            migrationBuilder.DropColumn(
                name: "OperatorID",
                table: "PatientLabRecords");
        }
    }
}
