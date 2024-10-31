using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimeonlyAccountAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeEnd",
                table: "PlayTime");

            migrationBuilder.DropColumn(
                name: "TimeStart",
                table: "PlayTime");

            migrationBuilder.DropColumn(
                name: "TotalTime",
                table: "PlayTime");

            migrationBuilder.DropColumn(
                name: "TotalTime",
                table: "Account");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "TimeTotal",
                table: "PlayTime",
                type: "time(0)",
                precision: 0,
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "TimeTotal",
                table: "Account",
                type: "time(0)",
                precision: 0,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeTotal",
                table: "PlayTime");

            migrationBuilder.DropColumn(
                name: "TimeTotal",
                table: "Account");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeEnd",
                table: "PlayTime",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStart",
                table: "PlayTime",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalTime",
                table: "PlayTime",
                type: "decimal(11,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalTime",
                table: "Account",
                type: "int",
                nullable: true);
        }
    }
}
