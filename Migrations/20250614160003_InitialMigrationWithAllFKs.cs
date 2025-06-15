using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mmrcis.Migrations
{
    
    public partial class InitialMigrationWithAllFKs : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CostRates",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostCode = table.Column<int>(type: "int", nullable: false),
                    CostAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CostType = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    IorE = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    AccountCode = table.Column<int>(type: "int", nullable: false),
                    RegisteredSince = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostRates", x => x.ID);
                    table.UniqueConstraint("AK_CostRates_AccountCode", x => x.AccountCode);
                    table.UniqueConstraint("AK_CostRates_CostCode", x => x.CostCode);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonType = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Qualification = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RegisteredSince = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Sex = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    BloodGroup = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Allergy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RegisteredSince = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RegisteredSince = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TicketFooters",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClinicName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HeaderLine1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HeaderLine2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HeaderLine3 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LabOPD = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketFooters", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TicketHeaders",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClinicName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HeaderLine1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HeaderLine2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HeaderLine3 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LabOPD = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketHeaders", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PersonID = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Persons_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Persons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseBills",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    OperatorID = table.Column<int>(type: "int", nullable: false),
                    IsPosted = table.Column<bool>(type: "bit", nullable: false),
                    IsVoided = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PostedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseBills", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExpenseBills_Persons_OperatorID",
                        column: x => x.OperatorID,
                        principalTable: "Persons",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ExpenseBills_Persons_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Persons",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "IncomeBills",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    OperatorID = table.Column<int>(type: "int", nullable: false),
                    IsPosted = table.Column<bool>(type: "bit", nullable: false),
                    IsVoided = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PostedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeBills", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IncomeBills_Persons_OperatorID",
                        column: x => x.OperatorID,
                        principalTable: "Persons",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_IncomeBills_Persons_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Persons",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PatientDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    DocumentContent = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientDocuments_Persons_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Persons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PatientSince = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Patients_Persons_PersonID",
                        column: x => x.PersonID,
                        principalTable: "Persons",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PatientVitals",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BP = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Oximeter = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    RespRate = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperatorID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientVitals", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientVitals_Persons_OperatorID",
                        column: x => x.OperatorID,
                        principalTable: "Persons",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PostingTransactions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountCode = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    OperatorID = table.Column<int>(type: "int", nullable: true),
                    CheckedPersonID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostingTransactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PostingTransactions_CostRates_AccountCode",
                        column: x => x.AccountCode,
                        principalTable: "CostRates",
                        principalColumn: "AccountCode");
                    table.ForeignKey(
                        name: "FK_PostingTransactions_Persons_CheckedPersonID",
                        column: x => x.CheckedPersonID,
                        principalTable: "Persons",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PostingTransactions_Persons_OperatorID",
                        column: x => x.OperatorID,
                        principalTable: "Persons",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CurrentStock = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinStockLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SupplierID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RegisteredSince = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseBillItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseBillID = table.Column<int>(type: "int", nullable: false),
                    CostCode = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseBillItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExpenseBillItems_CostRates_CostCode",
                        column: x => x.CostCode,
                        principalTable: "CostRates",
                        principalColumn: "CostCode");
                    table.ForeignKey(
                        name: "FK_ExpenseBillItems_ExpenseBills_ExpenseBillID",
                        column: x => x.ExpenseBillID,
                        principalTable: "ExpenseBills",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncomeBillItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomeBillID = table.Column<int>(type: "int", nullable: false),
                    CostCode = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeBillItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IncomeBillItems_CostRates_CostCode",
                        column: x => x.CostCode,
                        principalTable: "CostRates",
                        principalColumn: "CostCode");
                    table.ForeignKey(
                        name: "FK_IncomeBillItems_IncomeBills_IncomeBillID",
                        column: x => x.IncomeBillID,
                        principalTable: "IncomeBills",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientLabRecords",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    IncomeBillID = table.Column<int>(type: "int", nullable: false),
                    IsCollected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientLabRecords", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientLabRecords_IncomeBills_IncomeBillID",
                        column: x => x.IncomeBillID,
                        principalTable: "IncomeBills",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientLabRecords_Persons_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Persons",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    DoctorStaffID = table.Column<int>(type: "int", nullable: false),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    AppointmentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BookedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Appointments_Persons_DoctorStaffID",
                        column: x => x.DoctorStaffID,
                        principalTable: "Persons",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Appointments_Services_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "Services",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientCheckinOuts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    DoctorID = table.Column<int>(type: "int", nullable: true),
                    PatientVitalID = table.Column<int>(type: "int", nullable: true),
                    CIN_TIME = table.Column<DateTime>(type: "datetime2", nullable: true),
                    COUT_TIME = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsServed = table.Column<bool>(type: "bit", nullable: true),
                    IsBilled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientCheckinOuts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PatientCheckinOuts_PatientVitals_PatientVitalID",
                        column: x => x.PatientVitalID,
                        principalTable: "PatientVitals",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PatientCheckinOuts_Persons_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Persons",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PatientCheckinOuts_Persons_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Persons",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ClinicDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionID = table.Column<int>(type: "int", nullable: true),
                    DocumentContent = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ClinicDocuments_PostingTransactions_TransactionID",
                        column: x => x.TransactionID,
                        principalTable: "PostingTransactions",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorStaffID",
                table: "Appointments",
                column: "DoctorStaffID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientID",
                table: "Appointments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ServiceID",
                table: "Appointments",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PersonID",
                table: "AspNetUsers",
                column: "PersonID",
                unique: true,
                filter: "[PersonID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicDocuments_TransactionID",
                table: "ClinicDocuments",
                column: "TransactionID");

            migrationBuilder.CreateIndex(
                name: "IX_CostRates_AccountCode",
                table: "CostRates",
                column: "AccountCode",
                unique: true,
                filter: "[AccountCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CostRates_CostCode",
                table: "CostRates",
                column: "CostCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseBillItems_CostCode",
                table: "ExpenseBillItems",
                column: "CostCode");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseBillItems_ExpenseBillID",
                table: "ExpenseBillItems",
                column: "ExpenseBillID");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseBills_OperatorID",
                table: "ExpenseBills",
                column: "OperatorID");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseBills_PatientID",
                table: "ExpenseBills",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeBillItems_CostCode",
                table: "IncomeBillItems",
                column: "CostCode");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeBillItems_IncomeBillID",
                table: "IncomeBillItems",
                column: "IncomeBillID");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeBills_OperatorID",
                table: "IncomeBills",
                column: "OperatorID");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeBills_PatientID",
                table: "IncomeBills",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_SupplierID",
                table: "InventoryItems",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientCheckinOuts_DoctorID",
                table: "PatientCheckinOuts",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientCheckinOuts_PatientID",
                table: "PatientCheckinOuts",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientCheckinOuts_PatientVitalID",
                table: "PatientCheckinOuts",
                column: "PatientVitalID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDocuments_PatientID",
                table: "PatientDocuments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLabRecords_DoctorID",
                table: "PatientLabRecords",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLabRecords_IncomeBillID",
                table: "PatientLabRecords",
                column: "IncomeBillID");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PersonID",
                table: "Patients",
                column: "PersonID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientVitals_OperatorID",
                table: "PatientVitals",
                column: "OperatorID");

            migrationBuilder.CreateIndex(
                name: "IX_PostingTransactions_AccountCode",
                table: "PostingTransactions",
                column: "AccountCode");

            migrationBuilder.CreateIndex(
                name: "IX_PostingTransactions_CheckedPersonID",
                table: "PostingTransactions",
                column: "CheckedPersonID");

            migrationBuilder.CreateIndex(
                name: "IX_PostingTransactions_OperatorID",
                table: "PostingTransactions",
                column: "OperatorID");
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ClinicDocuments");

            migrationBuilder.DropTable(
                name: "ExpenseBillItems");

            migrationBuilder.DropTable(
                name: "IncomeBillItems");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "PatientCheckinOuts");

            migrationBuilder.DropTable(
                name: "PatientDocuments");

            migrationBuilder.DropTable(
                name: "PatientLabRecords");

            migrationBuilder.DropTable(
                name: "TicketFooters");

            migrationBuilder.DropTable(
                name: "TicketHeaders");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PostingTransactions");

            migrationBuilder.DropTable(
                name: "ExpenseBills");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "PatientVitals");

            migrationBuilder.DropTable(
                name: "IncomeBills");

            migrationBuilder.DropTable(
                name: "CostRates");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
