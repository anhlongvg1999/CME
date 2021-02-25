using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CME.DB.Migrations
{
    public partial class InitialDb4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "cat_Organizations",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedOnDate", "LastModifiedOnDate" },
                values: new object[] { new DateTime(2021, 2, 25, 0, 15, 21, 776, DateTimeKind.Local).AddTicks(7129), new DateTime(2021, 2, 25, 0, 15, 21, 777, DateTimeKind.Local).AddTicks(8495) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "cat_Organizations",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedOnDate", "LastModifiedOnDate" },
                values: new object[] { new DateTime(2021, 2, 25, 0, 8, 9, 389, DateTimeKind.Local).AddTicks(1885), new DateTime(2021, 2, 25, 0, 8, 9, 390, DateTimeKind.Local).AddTicks(3797) });
        }
    }
}
