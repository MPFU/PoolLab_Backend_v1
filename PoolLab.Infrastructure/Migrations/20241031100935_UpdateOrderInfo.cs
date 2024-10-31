using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CustomerPay",
                table: "Order",
                type: "decimal(11,0)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExcessCash",
                table: "Order",
                type: "decimal(11,0)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderBy",
                table: "Order",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerPay",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ExcessCash",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderBy",
                table: "Order");
        }
    }
}
