using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeEnd",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "TimeStart",
                table: "Subscription");

            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "Subscription",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NewPrice",
                table: "Subscription",
                type: "decimal(11,0)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "NewPrice",
                table: "Subscription");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeEnd",
                table: "Subscription",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStart",
                table: "Subscription",
                type: "datetime",
                nullable: true);
        }
    }
}
