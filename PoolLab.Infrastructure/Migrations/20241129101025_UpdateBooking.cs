using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRecurring",
                table: "Booking",
                type: "BIT",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RecurringId",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_RecurringId",
                table: "Booking",
                column: "RecurringId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Recurring",
                table: "Booking",
                column: "RecurringId",
                principalTable: "Booking",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Recurring",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_RecurringId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "IsRecurring",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "RecurringId",
                table: "Booking");
        }
    }
}
