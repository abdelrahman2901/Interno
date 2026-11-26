using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Inerno.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Product_Composet_IX : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_CategoryID",
                table: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryID_CreatedAt",
                table: "Product",
                columns: new[] { "CategoryID", "CreatedAt" },
                descending: new[] { false, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_CategoryID_CreatedAt",
                table: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryID",
                table: "Product",
                column: "CategoryID");
        }
    }
}
