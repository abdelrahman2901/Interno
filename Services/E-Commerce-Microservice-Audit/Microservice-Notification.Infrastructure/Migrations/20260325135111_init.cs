using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Microservice_Audit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    AuditID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedByUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedByUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.AuditID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audit");
        }
    }
}
