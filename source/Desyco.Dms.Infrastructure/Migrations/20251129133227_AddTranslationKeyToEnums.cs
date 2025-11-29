using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desyco.Dms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTranslationKeyToEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TranslationKey",
                table: "PaymentMethod",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranslationKey",
                table: "PaymentConceptType",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranslationKey",
                table: "EducationalLevelType",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranslationKey",
                table: "AttendanceStatus",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TranslationKey",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "TranslationKey",
                table: "PaymentConceptType");

            migrationBuilder.DropColumn(
                name: "TranslationKey",
                table: "EducationalLevelType");

            migrationBuilder.DropColumn(
                name: "TranslationKey",
                table: "AttendanceStatus");
        }
    }
}
