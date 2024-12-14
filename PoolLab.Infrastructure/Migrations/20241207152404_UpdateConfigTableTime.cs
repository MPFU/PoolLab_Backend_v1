using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConfigTableTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descript",
                table: "Event",
                type: "nvarchar(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DayLimit",
                table: "ConfigTable",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MonthLimit",
                table: "ConfigTable",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeAllowBook",
                table: "ConfigTable",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayLimit",
                table: "ConfigTable");

            migrationBuilder.DropColumn(
                name: "MonthLimit",
                table: "ConfigTable");

            migrationBuilder.DropColumn(
                name: "TimeAllowBook",
                table: "ConfigTable");

            migrationBuilder.AlterColumn<string>(
                name: "Descript",
                table: "Event",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)",
                oldNullable: true);
        }
    }
}
