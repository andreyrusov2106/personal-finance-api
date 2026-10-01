using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PersonalFinance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedCurrencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { new Guid("b0d4ce5d-2757-4699-948c-cfa72ba94001"), 643, "RUB" },
                    { new Guid("b0d4ce5d-2757-4699-948c-cfa72ba94002"), 840, "USD" },
                    { new Guid("b0d4ce5d-2757-4699-948c-cfa72ba94003"), 978, "EUR" },
                    { new Guid("b0d4ce5d-2757-4699-948c-cfa72ba94004"), 156, "CNY" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("b0d4ce5d-2757-4699-948c-cfa72ba94001"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("b0d4ce5d-2757-4699-948c-cfa72ba94002"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("b0d4ce5d-2757-4699-948c-cfa72ba94003"));

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: new Guid("b0d4ce5d-2757-4699-948c-cfa72ba94004"));
        }
    }
}
