using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_Commerce_Inerno.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class testUserseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Color",
                keyColumn: "ColorID",
                keyValue: new Guid("0a65996c-b895-42bc-af60-fac2f914376c"));

            migrationBuilder.DeleteData(
                table: "Color",
                keyColumn: "ColorID",
                keyValue: new Guid("0fb24adb-a140-4c50-9eb6-4d948408f30d"));

            migrationBuilder.DeleteData(
                table: "Color",
                keyColumn: "ColorID",
                keyValue: new Guid("179d4d45-54a7-48da-82f4-ad4673854bd2"));

            migrationBuilder.DeleteData(
                table: "Color",
                keyColumn: "ColorID",
                keyValue: new Guid("1b0856ec-68c8-49d5-9872-2a3f12c373ac"));

            migrationBuilder.DeleteData(
                table: "Color",
                keyColumn: "ColorID",
                keyValue: new Guid("220096a6-c67a-4640-83f2-ae526a2a542d"));

            migrationBuilder.DeleteData(
                table: "Color",
                keyColumn: "ColorID",
                keyValue: new Guid("438b0e6e-af1f-4bf1-9e18-ef5a2ddd94f3"));

            migrationBuilder.DeleteData(
                table: "Color",
                keyColumn: "ColorID",
                keyValue: new Guid("72a9cbb7-360c-4c75-97f7-680baf249a22"));


            migrationBuilder.DeleteData(
                table: "Color",
                keyColumn: "ColorID",
                keyValue: new Guid("97ca3fd7-7860-4961-8dde-d90cec05e3cf"));

            migrationBuilder.DeleteData(
                table: "Color",
                keyColumn: "ColorID",
                keyValue: new Guid("a3a07d13-c759-4920-a68a-47ef88e7594d"));

            migrationBuilder.DeleteData(
                table: "Size",
                keyColumn: "SizeID",
                keyValue: new Guid("4f0406b9-fe94-487f-9049-0b28da2c9274"));

            migrationBuilder.DeleteData(
                table: "Size",
                keyColumn: "SizeID",
                keyValue: new Guid("66a0c1c9-9db3-421a-87d0-f8c965687703"));

            migrationBuilder.DeleteData(
                table: "Size",
                keyColumn: "SizeID",
                keyValue: new Guid("67af942f-aeeb-4a4d-964b-0effc559368e"));

            migrationBuilder.DeleteData(
                table: "Size",
                keyColumn: "SizeID",
                keyValue: new Guid("730323ea-7e2e-4ec1-98f2-78fa1aa7c5e9"));

            migrationBuilder.DeleteData(
                table: "Size",
                keyColumn: "SizeID",
                keyValue: new Guid("d43285b3-b034-42cc-8795-e5fffb245654"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Color",
                columns: new[] { "ColorID", "ColorHexCode", "ColorName", "IsDeleted" },
                values: new object[,]
                {
                    { new Guid("0a65996c-b895-42bc-af60-fac2f914376c"), "#800080", "Purple", false },
                    { new Guid("0fb24adb-a140-4c50-9eb6-4d948408f30d"), "#FFFF00", "Yellow", false },
                    { new Guid("179d4d45-54a7-48da-82f4-ad4673854bd2"), "#000000", "Black", false },
                    { new Guid("1b0856ec-68c8-49d5-9872-2a3f12c373ac"), "#808080", "Gray", false },
                    { new Guid("220096a6-c67a-4640-83f2-ae526a2a542d"), "#008000", "Green", false },
                    { new Guid("438b0e6e-af1f-4bf1-9e18-ef5a2ddd94f3"), "#0000FF", "Blue", false },
                    { new Guid("72a9cbb7-360c-4c75-97f7-680baf249a22"), "#FFC0CB", "Pink", false },
                    { new Guid("91099764-4ff3-4a91-8f84-3bf913ca3b6c"), "#FF0000", "Red", false },
                    { new Guid("97ca3fd7-7860-4961-8dde-d90cec05e3cf"), "#FFFFFF", "White", false },
                    { new Guid("a3a07d13-c759-4920-a68a-47ef88e7594d"), "#FFA500", "Orange", false }
                });

            migrationBuilder.InsertData(
                table: "Size",
                columns: new[] { "SizeID", "IsDeleted", "SizeName" },
                values: new object[,]
                {
                    { new Guid("4f0406b9-fe94-487f-9049-0b28da2c9274"), false, "Small" },
                    { new Guid("66a0c1c9-9db3-421a-87d0-f8c965687703"), false, "XX Large" },
                    { new Guid("67af942f-aeeb-4a4d-964b-0effc559368e"), false, "Large" },
                    { new Guid("730323ea-7e2e-4ec1-98f2-78fa1aa7c5e9"), false, "Medium" },
                    { new Guid("d43285b3-b034-42cc-8795-e5fffb245654"), false, "X Large" }
                });
        }
    }
}
