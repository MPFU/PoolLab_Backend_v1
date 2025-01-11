using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderWithTableIssues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "TableIssues",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RepairStatus",
                table: "TableIssues",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalFee",
                table: "Order",
                type: "decimal(11,0)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TableIssuesId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_TableIssuesId",
                table: "Order",
                column: "TableIssuesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_TableIssues",
                table: "Order",
                column: "TableIssuesId",
                principalTable: "TableIssues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_TableIssues",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_TableIssuesId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "TableIssues");

            migrationBuilder.DropColumn(
                name: "RepairStatus",
                table: "TableIssues");

            migrationBuilder.DropColumn(
                name: "AdditionalFee",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TableIssuesId",
                table: "Order");
        }
    }
}
