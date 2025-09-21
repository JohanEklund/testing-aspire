using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace App1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Key", "FirstName", "LastName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "John", "Doe" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Jane", "Smith" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Key",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Key",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));
        }
    }
}
