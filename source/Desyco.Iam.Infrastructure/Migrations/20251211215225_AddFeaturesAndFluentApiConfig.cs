using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desyco.Iam.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFeaturesAndFluentApiConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "dls",
                table: "UserClaims",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FeatureId",
                schema: "dls",
                table: "UserClaims",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PermissionAction",
                schema: "dls",
                table: "UserClaims",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dls",
                table: "Roles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "dls",
                table: "RoleClaims",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FeatureId",
                schema: "dls",
                table: "RoleClaims",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PermissionAction",
                schema: "dls",
                table: "RoleClaims",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                schema: "dls",
                table: "RefreshTokens",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "JwtId",
                schema: "dls",
                table: "RefreshTokens",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Features",
                schema: "dls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Group = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Features_Code",
                schema: "dls",
                table: "Features",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Features",
                schema: "dls");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dls",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "FeatureId",
                schema: "dls",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "PermissionAction",
                schema: "dls",
                table: "UserClaims");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "dls",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "FeatureId",
                schema: "dls",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "PermissionAction",
                schema: "dls",
                table: "RoleClaims");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "dls",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                schema: "dls",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "JwtId",
                schema: "dls",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
