using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservice_Audit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingNewColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewValues",
                table: "Audit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldValues",
                table: "Audit",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewValues",
                table: "Audit");

            migrationBuilder.DropColumn(
                name: "OldValues",
                table: "Audit");
        }
    }
}
