using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookRental.NET.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "phoneNumber",
                table: "Users",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsAdmin", "Location", "Name", "Password", "PhoneNumber", "StartingDate", "Token" },
                values: new object[,]
                {
                    { 1, "WTAdmin@WT.com", true, "Oostzaan", "Admin WT", "admin", "1234", new DateTime(2023, 4, 17, 13, 57, 10, 973, DateTimeKind.Local).AddTicks(4927), "" },
                    { 2, "WTIntern@WT.com", false, "Groningen", "Intern WT", "intern", "5678", new DateTime(2023, 4, 17, 13, 57, 10, 973, DateTimeKind.Local).AddTicks(4965), "" },
                    { 3, "Client@ClientCompany.com", false, "Amsterdam", "Client User", "client", "password", new DateTime(2023, 4, 17, 13, 57, 10, 973, DateTimeKind.Local).AddTicks(4968), "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Users",
                newName: "phoneNumber");
        }
    }
}
