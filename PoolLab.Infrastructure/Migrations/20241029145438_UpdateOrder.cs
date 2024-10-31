using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BilliardTableId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_BilliardTableId",
                table: "Order",
                column: "BilliardTableId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_StoreID",
                table: "Order",
                column: "StoreID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_BilliardTable",
                table: "Order",
                column: "BilliardTableId",
                principalTable: "BilliardTable",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Store",
                table: "Order",
                column: "StoreID",
                principalTable: "Store",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_BilliardTable",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Store",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_BilliardTableId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_StoreID",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "BilliardTableId",
                table: "Order");
        }
    }
}
