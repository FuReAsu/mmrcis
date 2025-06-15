using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mmrcis.Migrations
{
    
    public partial class MakeIncomeBillIDNullableInPatientLabRecord : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IncomeBillID",
                table: "PatientLabRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IncomeBillID",
                table: "PatientLabRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
