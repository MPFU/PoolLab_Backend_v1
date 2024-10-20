using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConfigTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ConfigId",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_ConfigId",
                table: "Booking",
                column: "ConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_ConfigTable",
                table: "Booking",
                column: "ConfigId",
                principalTable: "ConfigTable",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_ConfigTable",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_ConfigId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "ConfigId",
                table: "Booking");
        }
    }
}
