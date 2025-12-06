using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desyco.Dms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAcademicYearStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicYearStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TranslationKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYearStatus", x => x.Id);
                });

            migrationBuilder.Sql("INSERT INTO AcademicYearStatus (Id, Name, TranslationKey) VALUES (1, 'Upcoming', 'ENUM_ACADEMICYEAR_UPCOMING')");
            migrationBuilder.Sql("INSERT INTO AcademicYearStatus (Id, Name, TranslationKey) VALUES (2, 'Current', 'ENUM_ACADEMICYEAR_CURRENT')");
            migrationBuilder.Sql("INSERT INTO AcademicYearStatus (Id, Name, TranslationKey) VALUES (3, 'Closed', 'ENUM_ACADEMICYEAR_CLOSED')");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AcademicYear");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AcademicYear",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Translation_LanguageId",
                table: "Translation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_Status",
                table: "Enrollment",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYear_Status",
                table: "AcademicYear",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicYear_AcademicYearStatus_Status",
                table: "AcademicYear",
                column: "Status",
                principalTable: "AcademicYearStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_EnrollmentStatus_Status",
                table: "Enrollment",
                column: "Status",
                principalTable: "EnrollmentStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Translation_Language_LanguageId",
                table: "Translation",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicYear_AcademicYearStatus_Status",
                table: "AcademicYear");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_EnrollmentStatus_Status",
                table: "Enrollment");

            migrationBuilder.DropForeignKey(
                name: "FK_Translation_Language_LanguageId",
                table: "Translation");

            migrationBuilder.DropTable(
                name: "AcademicYearStatus");

            migrationBuilder.DropIndex(
                name: "IX_Translation_LanguageId",
                table: "Translation");

            migrationBuilder.DropIndex(
                name: "IX_Enrollment_Status",
                table: "Enrollment");

            migrationBuilder.DropIndex(
                name: "IX_AcademicYear_Status",
                table: "AcademicYear");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AcademicYear");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AcademicYear",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
