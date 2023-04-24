using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookRental.NET.Migrations
{
    /// <inheritdoc />
    public partial class ReservationHasBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_BookId",
                table: "Reservations",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Books_BookId",
                table: "Reservations",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Books_BookId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_BookId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Reservations");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartingDate",
                value: new DateTime(2023, 4, 24, 13, 11, 13, 763, DateTimeKind.Local).AddTicks(4502));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartingDate",
                value: new DateTime(2023, 4, 24, 13, 11, 13, 763, DateTimeKind.Local).AddTicks(4535));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartingDate",
                value: new DateTime(2023, 4, 24, 13, 11, 13, 763, DateTimeKind.Local).AddTicks(4537));
        }
    }
}
