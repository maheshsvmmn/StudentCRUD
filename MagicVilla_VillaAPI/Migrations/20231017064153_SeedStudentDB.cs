using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Students_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedStudentDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Weight",
                table: "Students",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Class", "CreatedAt", "Name", "Weight" },
                values: new object[,]
                {
                    { 1, 10, new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7139), "Madhav", 50.399999999999999 },
                    { 2, 5, new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7153), "Suresh", 30.600000000000001 },
                    { 3, 7, new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7155), "Deepak", 40.399999999999999 },
                    { 4, 12, new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7157), "Hemant", 55.0 },
                    { 5, 3, new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7159), "Shashank", 25.399999999999999 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Students");

            migrationBuilder.AlterColumn<float>(
                name: "Weight",
                table: "Students",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
