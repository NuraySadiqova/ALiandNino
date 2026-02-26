using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AliAndNinoClone.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedBooksFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedDate", "IsDeleted", "Name" },
                values: new object[] { 1, new DateTime(2026, 2, 26, 21, 20, 7, 670, DateTimeKind.Local).AddTicks(2513), false, "Bədii Ədəbiyyat" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "CreatedDate", "ImageUrl", "IsDeleted", "Price", "StockCount", "Title" },
                values: new object[,]
                {
                    { 1, "Qurban Səid", 1, new DateTime(2026, 2, 26, 21, 20, 7, 670, DateTimeKind.Local).AddTicks(2601), "https://m.media-amazon.com/images/I/81dGZbz+H0L._AC_UF1000,1000_QL80_.jpg", false, 12.50m, 50, "Əli və Nino" },
                    { 2, "Corc Oruell", 1, new DateTime(2026, 2, 26, 21, 20, 7, 670, DateTimeKind.Local).AddTicks(2606), "https://m.media-amazon.com/images/I/71kxa1-0mfL._AC_UF1000,1000_QL80_.jpg", false, 10.99m, 30, "1984" },
                    { 3, "Fyodor Dostoyevski", 1, new DateTime(2026, 2, 26, 21, 20, 7, 670, DateTimeKind.Local).AddTicks(2607), "https://m.media-amazon.com/images/I/81WQO+zL6YL._AC_UF1000,1000_QL80_.jpg", false, 15.00m, 20, "Cinayət və Cəza" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
