using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateBookingConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeCancelBook",
                table: "ConfigTable",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Deposit",
                table: "Booking",
                type: "decimal(11,0)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeCancelBook",
                table: "ConfigTable");

            migrationBuilder.DropColumn(
                name: "Deposit",
                table: "Booking");
        }
    }
}
