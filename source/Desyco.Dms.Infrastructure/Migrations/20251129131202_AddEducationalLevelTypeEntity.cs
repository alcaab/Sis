using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desyco.Dms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEducationalLevelTypeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EducationalLevel",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LevelTypeId",
                table: "EducationalLevel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EducationalLevelType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationalLevelType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationalLevel_LevelTypeId",
                table: "EducationalLevel",
                column: "LevelTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationalLevel_EducationalLevelType_LevelTypeId",
                table: "EducationalLevel",
                column: "LevelTypeId",
                principalTable: "EducationalLevelType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationalLevel_EducationalLevelType_LevelTypeId",
                table: "EducationalLevel");

            migrationBuilder.DropTable(
                name: "EducationalLevelType");

            migrationBuilder.DropIndex(
                name: "IX_EducationalLevel_LevelTypeId",
                table: "EducationalLevel");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EducationalLevel");

            migrationBuilder.DropColumn(
                name: "LevelTypeId",
                table: "EducationalLevel");
        }
    }
}
