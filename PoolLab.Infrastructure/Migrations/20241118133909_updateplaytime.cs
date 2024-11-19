using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateplaytime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeTotal",
                table: "PlayTime");

            migrationBuilder.AddColumn<int>(
                name: "TypeCode",
                table: "Transaction",
                type: "int",
                nullable: true);

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
                type: "decimal(11,5)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeCode",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TimeEnd",
                table: "PlayTime");

            migrationBuilder.DropColumn(
                name: "TimeStart",
                table: "PlayTime");

            migrationBuilder.DropColumn(
                name: "TotalTime",
                table: "PlayTime");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "TimeTotal",
                table: "PlayTime",
                type: "time(0)",
                precision: 0,
                nullable: true);
        }
    }
}
