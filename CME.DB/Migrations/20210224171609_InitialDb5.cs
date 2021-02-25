using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CME.DB.Migrations
{
    public partial class InitialDb5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "cat_Organizations",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedOnDate", "LastModifiedOnDate" },
                values: new object[] { new DateTime(2021, 2, 25, 0, 16, 8, 416, DateTimeKind.Local).AddTicks(7466), new DateTime(2021, 2, 25, 0, 16, 8, 418, DateTimeKind.Local).AddTicks(3332) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "cat_Organizations",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedOnDate", "LastModifiedOnDate" },
                values: new object[] { new DateTime(2021, 2, 25, 0, 15, 21, 776, DateTimeKind.Local).AddTicks(7129), new DateTime(2021, 2, 25, 0, 15, 21, 777, DateTimeKind.Local).AddTicks(8495) });
        }
    }
}
