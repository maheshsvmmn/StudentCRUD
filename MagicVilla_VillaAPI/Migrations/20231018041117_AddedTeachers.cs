using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Students_API.Migrations
{
    /// <inheritdoc />
    public partial class AddedTeachers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<int>(type: "int", nullable: false),
                    HiringDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 18, 9, 41, 17, 432, DateTimeKind.Local).AddTicks(1158));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 18, 9, 41, 17, 432, DateTimeKind.Local).AddTicks(1169));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 18, 9, 41, 17, 432, DateTimeKind.Local).AddTicks(1171));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 18, 9, 41, 17, 432, DateTimeKind.Local).AddTicks(1173));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 18, 9, 41, 17, 432, DateTimeKind.Local).AddTicks(1174));

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "HiringDate", "Name", "Rating", "Salary", "Subject" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 10, 18, 9, 41, 17, 432, DateTimeKind.Local).AddTicks(1409), "Rakesh", 4.5, 45000, "Math" },
                    { 2, new DateTime(2023, 10, 18, 9, 41, 17, 432, DateTimeKind.Local).AddTicks(1412), "Ranjan", 3.3999999999999999, 34000, "Science" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7139));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7153));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7155));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7157));

            migrationBuilder.UpdateData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 10, 17, 12, 11, 52, 873, DateTimeKind.Local).AddTicks(7159));
        }
    }
}
