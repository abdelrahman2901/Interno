using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_Commerce_Inerno.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class testUserroleseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationRole",
                keyColumn: "RoleID",
                keyValue: new Guid("bcf2e140-cf5c-4352-a238-e04c7f220e64"));

            migrationBuilder.DeleteData(
                table: "ApplicationUserRoles",
                keyColumns: new[] { "RoleID", "UserID" },
                keyValues: new object[] { new Guid("bffbe3e7-4adf-4bb5-83db-6880f743d5da"), new Guid("d1a9b5c3-4f2e-4a6b-8c1d-9f3e2a1b5c3d") });

            migrationBuilder.DeleteData(
                table: "ApplicationRole",
                keyColumn: "RoleID",
                keyValue: new Guid("bffbe3e7-4adf-4bb5-83db-6880f743d5da"));

            migrationBuilder.DeleteData(
                table: "ApplicationUser",
                keyColumn: "UserID",
                keyValue: new Guid("d1a9b5c3-4f2e-4a6b-8c1d-9f3e2a1b5c3d"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ApplicationRole",
                columns: new[] { "RoleID", "RoleDescription", "RoleEnumID", "RoleName" },
                values: new object[,]
                {
                    { new Guid("bcf2e140-cf5c-4352-a238-e04c7f220e64"), "User role with limited permissions", 1, "Customer" },
                    { new Guid("bffbe3e7-4adf-4bb5-83db-6880f743d5da"), "Admin role with full permissions", 0, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "ApplicationUser",
                columns: new[] { "UserID", "Email", "Gender", "IsBlocked", "IsDeleted", "IsGmailVerified", "IsPhoneNumberVerified", "Password", "PersonName", "PhoneNumber", "RefreshToken", "RefreshTokenExpirtation" },
                values: new object[] { new Guid("d1a9b5c3-4f2e-4a6b-8c1d-9f3e2a1b5c3d"), "Admin@gmail.com", "Male", false, false, true, true, "Admin12", "Admin User", "+201066761644", null, null });

            migrationBuilder.InsertData(
                table: "ApplicationUserRoles",
                columns: new[] { "RoleID", "UserID", "IsDeleted" },
                values: new object[] { new Guid("bffbe3e7-4adf-4bb5-83db-6880f743d5da"), new Guid("d1a9b5c3-4f2e-4a6b-8c1d-9f3e2a1b5c3d"), false });
        }
    }
}
