using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desyco.Dms.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterDayOfWeekEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "CloseTime",
                table: "DayOfWeek",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "DayOfWeek",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEndOfWeek",
                table: "DayOfWeek",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSchoolDay",
                table: "DayOfWeek",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DayOfWeek",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "OpenTime",
                table: "DayOfWeek",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortName",
                table: "DayOfWeek",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "DayOfWeek",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "DayOfWeek");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "DayOfWeek");

            migrationBuilder.DropColumn(
                name: "IsEndOfWeek",
                table: "DayOfWeek");

            migrationBuilder.DropColumn(
                name: "IsSchoolDay",
                table: "DayOfWeek");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DayOfWeek");

            migrationBuilder.DropColumn(
                name: "OpenTime",
                table: "DayOfWeek");

            migrationBuilder.DropColumn(
                name: "ShortName",
                table: "DayOfWeek");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "DayOfWeek");
        }
    }
}
