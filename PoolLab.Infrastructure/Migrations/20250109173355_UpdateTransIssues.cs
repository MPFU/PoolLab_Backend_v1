using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransIssues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_TableIssues",
                table: "Order");

            migrationBuilder.AddColumn<Guid>(
                name: "TableIssuesId",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TableIssuesId",
                table: "Transaction",
                column: "TableIssuesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_TableIssues.",
                table: "Order",
                column: "TableIssuesId",
                principalTable: "TableIssues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_TableIssues",
                table: "Transaction",
                column: "TableIssuesId",
                principalTable: "TableIssues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_TableIssues.",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_TableIssues",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_TableIssuesId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TableIssuesId",
                table: "Transaction");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_TableIssues",
                table: "Order",
                column: "TableIssuesId",
                principalTable: "TableIssues",
                principalColumn: "Id");
        }
    }
}
