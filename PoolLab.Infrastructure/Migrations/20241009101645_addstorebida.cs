using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addstorebida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BilliardTable_StoreID",
                table: "BilliardTable",
                column: "StoreID");

            migrationBuilder.AddForeignKey(
                name: "FK_BilliardTable_Store",
                table: "BilliardTable",
                column: "StoreID",
                principalTable: "Store",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BilliardTable_Store",
                table: "BilliardTable");

            migrationBuilder.DropIndex(
                name: "IX_BilliardTable_StoreID",
                table: "BilliardTable");
        }
    }
}
