using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Key",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Id",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Key",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Id",
                value: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Users");
        }
    }
}
