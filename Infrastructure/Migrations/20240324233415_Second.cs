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
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Capacity", "CreatedAt", "UpdatedAt" },
                values: new object[] { 20, new DateTime(2024, 3, 24, 23, 34, 13, 262, DateTimeKind.Utc).AddTicks(1848), new DateTime(2024, 3, 24, 23, 34, 13, 262, DateTimeKind.Utc).AddTicks(1852) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "HashedPassword", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 3, 24, 23, 34, 13, 149, DateTimeKind.Utc).AddTicks(6355), "$2a$11$moRLI6gzWKiboiaOTkmd9eQjXoX8VvXImcURcBahTcZ5pGWioKme.", new DateTime(2024, 3, 24, 23, 34, 13, 149, DateTimeKind.Utc).AddTicks(6356) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Courses");

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 3, 24, 16, 34, 22, 816, DateTimeKind.Utc).AddTicks(9927), new DateTime(2024, 3, 24, 16, 34, 22, 816, DateTimeKind.Utc).AddTicks(9934) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreatedAt", "HashedPassword", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 3, 24, 16, 34, 22, 661, DateTimeKind.Utc).AddTicks(7043), "$2a$11$6lASmv1aTlMhTcH3fuwfbujuzKSV40Akf0UjCRkOs/mz/5m8r7j4u", new DateTime(2024, 3, 24, 16, 34, 22, 661, DateTimeKind.Utc).AddTicks(7044) });
        }
    }
}
