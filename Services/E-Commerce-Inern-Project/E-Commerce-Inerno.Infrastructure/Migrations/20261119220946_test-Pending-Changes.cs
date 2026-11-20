using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Inerno.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class testPendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Product");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ProductRates_Rating",
                table: "ProductRates",
                sql: "[Rating] BETWEEN 1 AND 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ProductRates_Rating",
                table: "ProductRates");

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Product",
                type: "float",
                maxLength: 10,
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
