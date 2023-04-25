using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookRental.NET.Migrations
{
    /// <inheritdoc />
    public partial class NullableDatetimeLoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Loans",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartingDate",
                value: new DateTime(2023, 4, 25, 15, 20, 22, 464, DateTimeKind.Local).AddTicks(5804));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartingDate",
                value: new DateTime(2023, 4, 25, 15, 20, 22, 464, DateTimeKind.Local).AddTicks(5837));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartingDate",
                value: new DateTime(2023, 4, 25, 15, 20, 22, 464, DateTimeKind.Local).AddTicks(5840));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Loans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartingDate",
                value: new DateTime(2023, 4, 24, 13, 13, 41, 485, DateTimeKind.Local).AddTicks(8688));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartingDate",
                value: new DateTime(2023, 4, 24, 13, 13, 41, 485, DateTimeKind.Local).AddTicks(8722));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartingDate",
                value: new DateTime(2023, 4, 24, 13, 13, 41, 485, DateTimeKind.Local).AddTicks(8724));
        }
    }
}
