using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Inerno.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Removed_Constrain_Phonenumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_ApplicationUser_PhoneNumber",
                table: "ApplicationUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_ApplicationUser_PhoneNumber",
                table: "ApplicationUser",
                sql: "LEN(PhoneNumber) = 13\r\nAND LEFT(PhoneNumber, 3) = '+20'\r\nAND SUBSTRING(PhoneNumber, 4, 1) = '1'\r\nAND SUBSTRING(PhoneNumber, 5, 1) IN ('0', '1', '2', '5')\r\nAND PhoneNumber NOT LIKE '%[^0-9+]%'");
        }
    }
}
