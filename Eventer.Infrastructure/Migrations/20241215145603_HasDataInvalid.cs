using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Eventer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HasDataInvalid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("80ee3df0-3b59-4504-a301-e4c4a2a7800d"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e8be62e0-e5f9-4003-b984-cab574b7eb42"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("80ee3df0-3b59-4504-a301-e4c4a2a7800d"), "Test description", "Test category" },
                    { new Guid("e8be62e0-e5f9-4003-b984-cab574b7eb42"), "Тестовое описание", "Тестовая категория" }
                });
        }
    }
}
