using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGroupProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductTypeId",
                table: "GroupProduct",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupProduct_ProductTypeId",
                table: "GroupProduct",
                column: "ProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupProducts_ProductType",
                table: "GroupProduct",
                column: "ProductTypeId",
                principalTable: "ProductType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupProducts_ProductType",
                table: "GroupProduct");

            migrationBuilder.DropIndex(
                name: "IX_GroupProduct_ProductTypeId",
                table: "GroupProduct");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                table: "GroupProduct");
        }
    }
}
