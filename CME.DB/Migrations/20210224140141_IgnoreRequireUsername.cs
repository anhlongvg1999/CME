using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CME.DB.Migrations
{
    public partial class IgnoreRequireUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "auth_Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "cat_Organizations",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedOnDate", "LastModifiedOnDate" },
                values: new object[] { new DateTime(2021, 2, 24, 21, 1, 40, 382, DateTimeKind.Local).AddTicks(9256), new DateTime(2021, 2, 24, 21, 1, 40, 384, DateTimeKind.Local).AddTicks(1239) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "auth_Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "cat_Organizations",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedOnDate", "LastModifiedOnDate" },
                values: new object[] { new DateTime(2021, 2, 24, 0, 48, 10, 314, DateTimeKind.Local).AddTicks(6836), new DateTime(2021, 2, 24, 0, 48, 10, 315, DateTimeKind.Local).AddTicks(8876) });
        }
    }
}
