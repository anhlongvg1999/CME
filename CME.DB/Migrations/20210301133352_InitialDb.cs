using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CME.DB.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cat_Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cat_Titles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_Titles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cat_TrainingForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_TrainingForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cat_Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cat_Departments_cat_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "cat_Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cat_TrainingSubjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TrainingFormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_TrainingSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cat_TrainingSubjects_cat_TrainingForms_TrainingFormId",
                        column: x => x.TrainingFormId,
                        principalTable: "cat_TrainingForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPrograms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrainingFormId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingPrograms_cat_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "cat_Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingPrograms_cat_TrainingForms_TrainingFormId",
                        column: x => x.TrainingFormId,
                        principalTable: "cat_TrainingForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "auth_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificateNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auth_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_auth_Users_cat_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "cat_Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_auth_Users_cat_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "cat_Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_auth_Users_cat_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "cat_Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainingProgram_User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingProgramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingSubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingProgram_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingProgram_User_auth_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "auth_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingProgram_User_TrainingPrograms_TrainingProgramId",
                        column: x => x.TrainingProgramId,
                        principalTable: "TrainingPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "cat_Organizations",
                columns: new[] { "Id", "Address", "ApplicationId", "Code", "CreatedByUserId", "CreatedOnDate", "LastModifiedByUserId", "LastModifiedOnDate", "Name", "ParentId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "12 Chu Văn An, Điện Bàn, Ba Đình, Hà Nội", null, "BVXP", null, new DateTime(2021, 3, 1, 20, 33, 51, 415, DateTimeKind.Local).AddTicks(6050), null, new DateTime(2021, 3, 1, 20, 33, 51, 416, DateTimeKind.Local).AddTicks(6243), "BV Xanh Pôn", null });

            migrationBuilder.InsertData(
                table: "cat_TrainingForms",
                columns: new[] { "Id", "ApplicationId", "Code", "CreatedByUserId", "CreatedOnDate", "LastModifiedByUserId", "LastModifiedOnDate", "Name", "ParentId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-200000000001"), null, "HTHN", null, new DateTime(2021, 3, 1, 20, 33, 51, 418, DateTimeKind.Local).AddTicks(4022), null, new DateTime(2021, 3, 1, 20, 33, 51, 418, DateTimeKind.Local).AddTicks(4035), "Hội thảo, hội nghị", null });

            migrationBuilder.InsertData(
                table: "cat_TrainingSubjects",
                columns: new[] { "Id", "Amount", "ApplicationId", "Code", "CreatedByUserId", "CreatedOnDate", "LastModifiedByUserId", "LastModifiedOnDate", "Name", "Order", "ParentId", "TrainingFormId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-300000000001"), 4.0, null, null, null, new DateTime(2021, 3, 1, 20, 33, 51, 418, DateTimeKind.Local).AddTicks(8122), null, new DateTime(2021, 3, 1, 20, 33, 51, 418, DateTimeKind.Local).AddTicks(8130), "Người tham dự", 0, null, new Guid("00000000-0000-0000-0000-200000000001") });

            migrationBuilder.InsertData(
                table: "cat_TrainingSubjects",
                columns: new[] { "Id", "Amount", "ApplicationId", "Code", "CreatedByUserId", "CreatedOnDate", "LastModifiedByUserId", "LastModifiedOnDate", "Name", "Order", "ParentId", "TrainingFormId" },
                values: new object[] { new Guid("00000000-0000-0000-0000-300000000002"), 8.0, null, null, null, new DateTime(2021, 3, 1, 20, 33, 51, 418, DateTimeKind.Local).AddTicks(8313), null, new DateTime(2021, 3, 1, 20, 33, 51, 418, DateTimeKind.Local).AddTicks(8316), "Chủ trì", 0, null, new Guid("00000000-0000-0000-0000-200000000001") });

            migrationBuilder.CreateIndex(
                name: "IX_auth_Users_DepartmentId",
                table: "auth_Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_auth_Users_OrganizationId",
                table: "auth_Users",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_auth_Users_TitleId",
                table: "auth_Users",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_cat_Departments_OrganizationId",
                table: "cat_Departments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_cat_TrainingSubjects_TrainingFormId",
                table: "cat_TrainingSubjects",
                column: "TrainingFormId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProgram_User_TrainingProgramId",
                table: "TrainingProgram_User",
                column: "TrainingProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProgram_User_UserId",
                table: "TrainingProgram_User",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPrograms_OrganizationId",
                table: "TrainingPrograms",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPrograms_TrainingFormId",
                table: "TrainingPrograms",
                column: "TrainingFormId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cat_TrainingSubjects");

            migrationBuilder.DropTable(
                name: "TrainingProgram_User");

            migrationBuilder.DropTable(
                name: "auth_Users");

            migrationBuilder.DropTable(
                name: "TrainingPrograms");

            migrationBuilder.DropTable(
                name: "cat_Departments");

            migrationBuilder.DropTable(
                name: "cat_Titles");

            migrationBuilder.DropTable(
                name: "cat_TrainingForms");

            migrationBuilder.DropTable(
                name: "cat_Organizations");
        }
    }
}
