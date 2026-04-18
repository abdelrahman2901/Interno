using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Inern_Project.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixing23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_AspNetUsers_UserID",
                table: "Cart");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("57f2f45d-b062-40eb-beaf-0cf31e196d64"));

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_AspNetUsers_UserID",
                table: "Cart",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_AspNetUsers_UserID",
                table: "Cart");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsBlocked", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PersonName", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpirtation", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("57f2f45d-b062-40eb-beaf-0cf31e196d64"), 0, "aa5b33dd-c16c-4302-bbd9-18bc3bddb94e", "admin@gmail.com", true, false, false, false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEOUyqj/tuInhSxHiTxOv2lmO3R2GF1wjB55BEAY81ZhWStcKoUAWL5yEs1/Ogc0FCg==", "admin", null, false, null, null, "4AA279DC-CFC5-4B21-96C7-52AABB89F127", false, "admin@gmail.com" });

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_AspNetUsers_UserID",
                table: "Cart",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
