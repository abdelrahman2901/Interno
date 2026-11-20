using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Inerno.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Check_EnumConstrains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Payments_PaymentMethod",
                table: "Payments",
                sql: "[PaymentMethod] IN ('Cash', 'CreditCard')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Payments_Status",
                table: "Payments",
                sql: "[Status] IN ('Approved', 'Rejected', 'Pending')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Order_OrderStatus",
                table: "Orders",
                sql: "[OrderStatus] IN ('Cancelled', 'Delivered', 'Shipped' , 'Processing')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_OrderCoupon_DiscountType",
                table: "OrderCoupon",
                sql: "[DiscountType] IN ('Percentage', 'Fixed')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ApplicationUser_Gender",
                table: "ApplicationUser",
                sql: "[Gender] IN ('Male', 'Female')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Payments_PaymentMethod",
                table: "Payments");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Payments_Status",
                table: "Payments");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Order_OrderStatus",
                table: "Orders");

            migrationBuilder.DropCheckConstraint(
                name: "CK_OrderCoupon_DiscountType",
                table: "OrderCoupon");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ApplicationUser_Gender",
                table: "ApplicationUser");
        }
    }
}
