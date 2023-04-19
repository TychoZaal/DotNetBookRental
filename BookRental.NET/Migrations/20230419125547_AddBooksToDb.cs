using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookRental.NET.Migrations
{
    /// <inheritdoc />
    public partial class AddBooksToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "ISBN", "Title" },
                values: new object[,]
                {
                    { 1, "George R. R. Martin", "1234-5678-4200", "Knight of the Seven Kingdoms" },
                    { 2, "Leigh Bardugo", "8972-2387-2873", "Shadow and Bone" },
                    { 3, "J. R. R. Tolkien", "2890-5498-1283", "Lord of the Rings" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartingDate",
                value: new DateTime(2023, 4, 19, 14, 55, 47, 138, DateTimeKind.Local).AddTicks(7201));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartingDate",
                value: new DateTime(2023, 4, 19, 14, 55, 47, 138, DateTimeKind.Local).AddTicks(7242));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartingDate",
                value: new DateTime(2023, 4, 19, 14, 55, 47, 138, DateTimeKind.Local).AddTicks(7245));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartingDate",
                value: new DateTime(2023, 4, 17, 13, 57, 10, 973, DateTimeKind.Local).AddTicks(4927));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartingDate",
                value: new DateTime(2023, 4, 17, 13, 57, 10, 973, DateTimeKind.Local).AddTicks(4965));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartingDate",
                value: new DateTime(2023, 4, 17, 13, 57, 10, 973, DateTimeKind.Local).AddTicks(4968));
        }
    }
}
