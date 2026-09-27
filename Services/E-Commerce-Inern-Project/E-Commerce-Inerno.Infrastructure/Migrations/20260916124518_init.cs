using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_Commerce_Inern_Project.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationRole",
                columns: table => new
                {
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleEnumID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationRole", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsGmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsPhoneNumberVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpirtation = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentCategoryID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: true),
                    CategoryImageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HashImage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_Category_Category_ParentCategoryID",
                        column: x => x.ParentCategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID");
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    CityID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityID);
                });

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    ColorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ColorName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ColorHexCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.ColorID);
                });

            migrationBuilder.CreateTable(
                name: "OrderCoupon",
                columns: table => new
                {
                    OrderCouponID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    CouponCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", maxLength: 10, nullable: false),
                    DiscountType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCoupon", x => x.OrderCouponID);
                });

            migrationBuilder.CreateTable(
                name: "Size",
                columns: table => new
                {
                    SizeID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    SizeName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Size", x => x.SizeID);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserRoles",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserRoles", x => new { x.UserID, x.RoleID });
                    table.ForeignKey(
                        name: "FK_ApplicationUserRoles_ApplicationRole_RoleID",
                        column: x => x.RoleID,
                        principalTable: "ApplicationRole",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserRoles_ApplicationUser_UserID",
                        column: x => x.UserID,
                        principalTable: "ApplicationUser",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CartID);
                    table.ForeignKey(
                        name: "FK_Cart_ApplicationUser_UserID",
                        column: x => x.UserID,
                        principalTable: "ApplicationUser",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", maxLength: 100, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_Payments_ApplicationUser_UserID",
                        column: x => x.UserID,
                        principalTable: "ApplicationUser",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    AreaID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false),
                    CityID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false),
                    AreaName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.AreaID);
                    table.ForeignKey(
                        name: "FK_Area_City_CityID",
                        column: x => x.CityID,
                        principalTable: "City",
                        principalColumn: "CityID");
                });

            migrationBuilder.CreateTable(
                name: "BannerSlides",
                columns: table => new
                {
                    BannerSlideID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CTA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BackgroundColorID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 200, nullable: false),
                    AccentColorID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 20, nullable: false),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerSlides", x => x.BannerSlideID);
                    table.ForeignKey(
                        name: "FK_BannerSlides_Color_AccentColorID",
                        column: x => x.AccentColorID,
                        principalTable: "Color",
                        principalColumn: "ColorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannerSlides_Color_BackgroundColorID",
                        column: x => x.BackgroundColorID,
                        principalTable: "Color",
                        principalColumn: "ColorID");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", maxLength: 10, nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", maxLength: 10, nullable: true),
                    Stock = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    Rating = table.Column<double>(type: "float", maxLength: 10, nullable: false),
                    TotalRating = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    SizeID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    ColorID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductImageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    HashImage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID");
                    table.ForeignKey(
                        name: "FK_Product_Color_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Color",
                        principalColumn: "ColorID");
                    table.ForeignKey(
                        name: "FK_Product_Size_SizeID",
                        column: x => x.SizeID,
                        principalTable: "Size",
                        principalColumn: "SizeID");
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    AddressLabel = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MainAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BackUpAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CityID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    AreaID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    BackUpPhoneNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressID);
                    table.ForeignKey(
                        name: "FK_Address_ApplicationUser_UserID",
                        column: x => x.UserID,
                        principalTable: "ApplicationUser",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_Address_Area_AreaID",
                        column: x => x.AreaID,
                        principalTable: "Area",
                        principalColumn: "AreaID");
                    table.ForeignKey(
                        name: "FK_Address_City_CityID",
                        column: x => x.CityID,
                        principalTable: "City",
                        principalColumn: "CityID");
                });

            migrationBuilder.CreateTable(
                name: "ShippingCosts",
                columns: table => new
                {
                    ShippingCostID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", maxLength: 10, nullable: false),
                    AraeID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingCosts", x => x.ShippingCostID);
                    table.ForeignKey(
                        name: "FK_ShippingCosts_Area_AraeID",
                        column: x => x.AraeID,
                        principalTable: "Area",
                        principalColumn: "AreaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartItemID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    CartID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.CartItemID);
                    table.ForeignKey(
                        name: "FK_CartItems_Cart_CartID",
                        column: x => x.CartID,
                        principalTable: "Cart",
                        principalColumn: "CartID");
                    table.ForeignKey(
                        name: "FK_CartItems_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "ProductRates",
                columns: table => new
                {
                    RateID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 200, nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 200, nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 200, nullable: false),
                    Rating = table.Column<double>(type: "float", maxLength: 10, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRates", x => x.RateID);
                    table.ForeignKey(
                        name: "FK_ProductRates_ApplicationUser_UserID",
                        column: x => x.UserID,
                        principalTable: "ApplicationUser",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_ProductRates_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "WishList",
                columns: table => new
                {
                    WishlistID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishList", x => x.WishlistID);
                    table.ForeignKey(
                        name: "FK_WishList_ApplicationUser_UserID",
                        column: x => x.UserID,
                        principalTable: "ApplicationUser",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_WishList_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    AddressID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", maxLength: 50, nullable: false),
                    ShippingCostID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderCouponID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: true),
                    OrderStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Address_AddressID",
                        column: x => x.AddressID,
                        principalTable: "Address",
                        principalColumn: "AddressID");
                    table.ForeignKey(
                        name: "FK_Orders_ApplicationUser_UserID",
                        column: x => x.UserID,
                        principalTable: "ApplicationUser",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_Orders_OrderCoupon_OrderCouponID",
                        column: x => x.OrderCouponID,
                        principalTable: "OrderCoupon",
                        principalColumn: "OrderCouponID");
                    table.ForeignKey(
                        name: "FK_Orders_Payments_PaymentID",
                        column: x => x.PaymentID,
                        principalTable: "Payments",
                        principalColumn: "PaymentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_ShippingCosts_ShippingCostID",
                        column: x => x.ShippingCostID,
                        principalTable: "ShippingCosts",
                        principalColumn: "ShippingCostID");
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemID);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID");
                    table.ForeignKey(
                        name: "FK_OrderItems_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID");
                });

            migrationBuilder.InsertData(
                table: "ApplicationRole",
                columns: new[] { "RoleID", "RoleDescription", "RoleEnumID", "RoleName" },
                values: new object[,]
                {
                    { new Guid("bcf2e140-cf5c-4352-a238-e04c7f220e64"), "User role with limited permissions", 1, "Customer" },
                    { new Guid("bffbe3e7-4adf-4bb5-83db-6880f743d5da"), "Admin role with full permissions", 0, "Admin" }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Address_AreaID",
                table: "Address",
                column: "AreaID");

            migrationBuilder.CreateIndex(
                name: "IX_Address_CityID",
                table: "Address",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Address_UserID",
                table: "Address",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserRoles_RoleID",
                table: "ApplicationUserRoles",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Area_CityID",
                table: "Area",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_BannerSlides_AccentColorID",
                table: "BannerSlides",
                column: "AccentColorID");

            migrationBuilder.CreateIndex(
                name: "IX_BannerSlides_BackgroundColorID",
                table: "BannerSlides",
                column: "BackgroundColorID");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserID",
                table: "Cart",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartID",
                table: "CartItems",
                column: "CartID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductID",
                table: "CartItems",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentCategoryID",
                table: "Category",
                column: "ParentCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Color_ColorName",
                table: "Color",
                column: "ColorName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderID",
                table: "OrderItems",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductID",
                table: "OrderItems",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressID",
                table: "Orders",
                column: "AddressID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderCouponID",
                table: "Orders",
                column: "OrderCouponID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentID",
                table: "Orders",
                column: "PaymentID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingCostID",
                table: "Orders",
                column: "ShippingCostID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserID",
                table: "Orders",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserID",
                table: "Payments",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryID",
                table: "Product",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ColorID",
                table: "Product",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SizeID",
                table: "Product",
                column: "SizeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRates_ProductID",
                table: "ProductRates",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRates_UserID",
                table: "ProductRates",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingCosts_AraeID",
                table: "ShippingCosts",
                column: "AraeID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WishList_ProductID",
                table: "WishList",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_WishList_UserID",
                table: "WishList",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserRoles");

            migrationBuilder.DropTable(
                name: "BannerSlides");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ProductRates");

            migrationBuilder.DropTable(
                name: "WishList");

            migrationBuilder.DropTable(
                name: "ApplicationRole");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "OrderCoupon");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "ShippingCosts");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropTable(
                name: "Size");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "Area");

            migrationBuilder.DropTable(
                name: "City");
        }
    }
}
