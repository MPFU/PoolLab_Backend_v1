using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Company",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Store_Company",
                table: "Store");

            migrationBuilder.DropIndex(
                name: "IX_Store_CompanyID",
                table: "Store");

            migrationBuilder.DropIndex(
                name: "IX_Account_CompanyID",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "Account");

            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedCost",
                table: "TableMaintenance",
                type: "decimal(11,0)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedCost",
                table: "TableMaintenance");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyID",
                table: "Store",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyID",
                table: "Account",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Store_CompanyID",
                table: "Store",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Account_CompanyID",
                table: "Account",
                column: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Company",
                table: "Account",
                column: "CompanyID",
                principalTable: "Company",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_Company",
                table: "Store",
                column: "CompanyID",
                principalTable: "Company",
                principalColumn: "Id");
        }
    }
}
