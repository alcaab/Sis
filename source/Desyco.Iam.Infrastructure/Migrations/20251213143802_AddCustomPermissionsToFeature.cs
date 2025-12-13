using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desyco.Iam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomPermissionsToFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomPermissions",
                schema: "dls",
                table: "Features",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomPermissions",
                schema: "dls",
                table: "Features");
        }
    }
}
