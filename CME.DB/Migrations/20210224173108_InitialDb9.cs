using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CME.DB.Migrations
{
    public partial class InitialDb9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TraningProgram_User");

            migrationBuilder.CreateTable(
                name: "TrainingProgram_User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrainingProgramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
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

            migrationBuilder.UpdateData(
                table: "cat_Organizations",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedOnDate", "LastModifiedOnDate" },
                values: new object[] { new DateTime(2021, 2, 25, 0, 31, 7, 990, DateTimeKind.Local).AddTicks(7175), new DateTime(2021, 2, 25, 0, 31, 7, 992, DateTimeKind.Local).AddTicks(570) });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProgram_User_TrainingProgramId",
                table: "TrainingProgram_User",
                column: "TrainingProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProgram_User_UserId",
                table: "TrainingProgram_User",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingProgram_User");

            migrationBuilder.CreateTable(
                name: "TraningProgram_User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModifiedOnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrainingProgramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraningProgram_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TraningProgram_User_auth_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "auth_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TraningProgram_User_TrainingPrograms_TrainingProgramId",
                        column: x => x.TrainingProgramId,
                        principalTable: "TrainingPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "cat_Organizations",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedOnDate", "LastModifiedOnDate" },
                values: new object[] { new DateTime(2021, 2, 25, 0, 17, 11, 964, DateTimeKind.Local).AddTicks(9668), new DateTime(2021, 2, 25, 0, 17, 11, 966, DateTimeKind.Local).AddTicks(937) });

            migrationBuilder.CreateIndex(
                name: "IX_TraningProgram_User_TrainingProgramId",
                table: "TraningProgram_User",
                column: "TrainingProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_TraningProgram_User_UserId",
                table: "TraningProgram_User",
                column: "UserId");
        }
    }
}
