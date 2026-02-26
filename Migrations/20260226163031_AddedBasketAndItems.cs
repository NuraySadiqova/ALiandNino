using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AliAndNinoClone.Migrations
{
    /// <inheritdoc />
    public partial class AddedBasketAndItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_BookId",
                table: "BasketItems",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Books_BookId",
                table: "BasketItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Books_BookId",
                table: "BasketItems");

            migrationBuilder.DropIndex(
                name: "IX_BasketItems_BookId",
                table: "BasketItems");
        }
    }
}
