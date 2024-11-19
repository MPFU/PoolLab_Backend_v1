using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderPlaytime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayTime_Order",
                table: "PlayTime");

            migrationBuilder.DropIndex(
                name: "IX_PlayTime_OrderID",
                table: "PlayTime");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "PlayTime");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "PlayTime",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlayTimeId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_PlayTimeId",
                table: "Order",
                column: "PlayTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_PlayTime",
                table: "Order",
                column: "PlayTimeId",
                principalTable: "PlayTime",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_PlayTime",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_PlayTimeId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PlayTime");

            migrationBuilder.DropColumn(
                name: "PlayTimeId",
                table: "Order");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderID",
                table: "PlayTime",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayTime_OrderID",
                table: "PlayTime",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayTime_Order",
                table: "PlayTime",
                column: "OrderID",
                principalTable: "Order",
                principalColumn: "Id");
        }
    }
}
