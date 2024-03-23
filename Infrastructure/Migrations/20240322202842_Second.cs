using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 3, 22, 20, 28, 40, 498, DateTimeKind.Utc).AddTicks(6049), new DateTime(2024, 3, 22, 20, 28, 40, 498, DateTimeKind.Utc).AddTicks(6053) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "FirstName", "HashedPassword", "LastName", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 3, 22, 20, 28, 40, 384, DateTimeKind.Utc).AddTicks(6698), "امیر", "$2a$11$gYmPdkfik9QlklIFGgyVR.PMLRKm1z16JXDxlyBHSKjlhmcs9cXjy", "حسینی", new DateTime(2024, 3, 22, 20, 28, 40, 384, DateTimeKind.Utc).AddTicks(6700) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 3, 22, 15, 9, 23, 236, DateTimeKind.Utc).AddTicks(3012), new DateTime(2024, 3, 22, 15, 9, 23, 236, DateTimeKind.Utc).AddTicks(3015) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "HashedPassword", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 3, 22, 15, 9, 23, 63, DateTimeKind.Utc).AddTicks(9243), "$2a$11$XwHVx9d8Yr4wIp4dvZI7wexElt3nE.ahKUG5FmqW8JCPav7e8F7YK", new DateTime(2024, 3, 22, 15, 9, 23, 63, DateTimeKind.Utc).AddTicks(9245) });
        }
    }
}
