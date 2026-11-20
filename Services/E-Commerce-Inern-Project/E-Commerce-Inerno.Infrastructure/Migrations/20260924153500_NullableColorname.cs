using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Inerno.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NullableColorname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Color_ColorName",
                table: "Color");

            migrationBuilder.AlterColumn<string>(
                name: "ColorName",
                table: "Color",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Color_ColorName",
                table: "Color",
                column: "ColorName",
                unique: true,
                filter: "[ColorName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Color_ColorName",
                table: "Color");

            migrationBuilder.AlterColumn<string>(
                name: "ColorName",
                table: "Color",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Color_ColorName",
                table: "Color",
                column: "ColorName",
                unique: true);
        }
    }
}
