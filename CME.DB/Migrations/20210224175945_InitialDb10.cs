using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CME.DB.Migrations
{
    public partial class InitialDb10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrainingSubjectName",
                table: "TrainingProgram_User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "cat_Organizations",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedOnDate", "LastModifiedOnDate" },
                values: new object[] { new DateTime(2021, 2, 25, 0, 59, 44, 483, DateTimeKind.Local).AddTicks(5504), new DateTime(2021, 2, 25, 0, 59, 44, 484, DateTimeKind.Local).AddTicks(7446) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrainingSubjectName",
                table: "TrainingProgram_User");

            migrationBuilder.UpdateData(
                table: "cat_Organizations",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedOnDate", "LastModifiedOnDate" },
                values: new object[] { new DateTime(2021, 2, 25, 0, 31, 7, 990, DateTimeKind.Local).AddTicks(7175), new DateTime(2021, 2, 25, 0, 31, 7, 992, DateTimeKind.Local).AddTicks(570) });
        }
    }
}
