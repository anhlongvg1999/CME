using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CME.DB.Migrations
{
    public partial class InitialDb3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "cat_Organizations",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedOnDate", "LastModifiedOnDate", "Name" },
                values: new object[] { new DateTime(2021, 2, 25, 0, 8, 9, 389, DateTimeKind.Local).AddTicks(1885), new DateTime(2021, 2, 25, 0, 8, 9, 390, DateTimeKind.Local).AddTicks(3797), "BV Xanh Pôn" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "cat_Organizations",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                columns: new[] { "CreatedOnDate", "LastModifiedOnDate", "Name" },
                values: new object[] { new DateTime(2021, 2, 24, 21, 1, 40, 382, DateTimeKind.Local).AddTicks(9256), new DateTime(2021, 2, 24, 21, 1, 40, 384, DateTimeKind.Local).AddTicks(1239), "Bệnh viện Xanh Pôn" });
        }
    }
}
