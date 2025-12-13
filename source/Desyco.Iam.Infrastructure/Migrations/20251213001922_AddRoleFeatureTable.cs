using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desyco.Iam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleFeatureTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleFeatures",
                schema: "dls",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleFeatures", x => new { x.RoleId, x.FeatureId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleFeatures",
                schema: "dls");
        }
    }
}
