using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Inerno.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FIXING_ApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PersonName",
                table: "ApplicationUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "ApplicationUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ApplicationUser",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "ApplicationUser",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "ApplicationUser",
                columns: new[] { "UserID", "Email", "Gender", "IsBlocked", "IsDeleted", "IsGmailVerified", "IsPhoneNumberVerified", "Password", "PersonName", "PhoneNumber", "RefreshToken", "RefreshTokenExpirtation" },
                values: new object[] { new Guid("d1a9b5c3-4f2e-4a6b-8c1d-9f3e2a1b5c3d"), "Admin@gmail.com", "Male", false, false, true, true, "Admin12", "Admin User", "+201066761644", null, null });

            migrationBuilder.InsertData(
                table: "ApplicationUserRoles",
                columns: new[] { "RoleID", "UserID", "IsDeleted" },
                values: new object[] { new Guid("bffbe3e7-4adf-4bb5-83db-6880f743d5da"), new Guid("d1a9b5c3-4f2e-4a6b-8c1d-9f3e2a1b5c3d"), false });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Email",
                table: "ApplicationUser",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_PhoneNumber",
                table: "ApplicationUser",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_ApplicationUser_PhoneNumber",
                table: "ApplicationUser",
                sql: "LEN(PhoneNumber) = 13\r\nAND LEFT(PhoneNumber, 3) = '+20'\r\nAND SUBSTRING(PhoneNumber, 4, 1) = '1'\r\nAND SUBSTRING(PhoneNumber, 5, 1) IN ('0', '1', '2', '5')\r\nAND PhoneNumber NOT LIKE '%[^0-9+]%'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApplicationUser_Email",
                table: "ApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationUser_PhoneNumber",
                table: "ApplicationUser");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ApplicationUser_PhoneNumber",
                table: "ApplicationUser");

            migrationBuilder.DeleteData(
                table: "ApplicationUserRoles",
                keyColumns: new[] { "RoleID", "UserID" },
                keyValues: new object[] { new Guid("bffbe3e7-4adf-4bb5-83db-6880f743d5da"), new Guid("d1a9b5c3-4f2e-4a6b-8c1d-9f3e2a1b5c3d") });

            migrationBuilder.DeleteData(
                table: "ApplicationUser",
                keyColumn: "UserID",
                keyValue: new Guid("d1a9b5c3-4f2e-4a6b-8c1d-9f3e2a1b5c3d"));

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ApplicationUser");

            migrationBuilder.AlterColumn<string>(
                name: "PersonName",
                table: "ApplicationUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "ApplicationUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ApplicationUser",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
