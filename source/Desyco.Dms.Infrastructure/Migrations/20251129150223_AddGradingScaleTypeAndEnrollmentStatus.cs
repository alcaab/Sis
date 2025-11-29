using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desyco.Dms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGradingScaleTypeAndEnrollmentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "GradingScale",
                newName: "TypeId");

            migrationBuilder.CreateTable(
                name: "EnrollmentStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TranslationKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradingScaleType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TranslationKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradingScaleType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GradingScale_TypeId",
                table: "GradingScale",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GradingScale_GradingScaleType_TypeId",
                table: "GradingScale",
                column: "TypeId",
                principalTable: "GradingScaleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradingScale_GradingScaleType_TypeId",
                table: "GradingScale");

            migrationBuilder.DropTable(
                name: "EnrollmentStatus");

            migrationBuilder.DropTable(
                name: "GradingScaleType");

            migrationBuilder.DropIndex(
                name: "IX_GradingScale_TypeId",
                table: "GradingScale");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "GradingScale",
                newName: "Type");
        }
    }
}
