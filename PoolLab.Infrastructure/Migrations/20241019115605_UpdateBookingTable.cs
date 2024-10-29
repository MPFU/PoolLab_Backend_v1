using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AreaId",
                table: "Booking",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BilliardTypeArea",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BilliardTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AreaID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BilliardTypeArea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BilliardTypeArea_Area",
                        column: x => x.AreaID,
                        principalTable: "Area",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BilliardTypeArea_BilliardType",
                        column: x => x.BilliardTypeID,
                        principalTable: "BilliardType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_AreaId",
                table: "Booking",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_BilliardTypeArea_AreaID",
                table: "BilliardTypeArea",
                column: "AreaID");

            migrationBuilder.CreateIndex(
                name: "IX_BilliardTypeArea_BilliardTypeID",
                table: "BilliardTypeArea",
                column: "BilliardTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Area",
                table: "Booking",
                column: "AreaId",
                principalTable: "Area",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Area",
                table: "Booking");

            migrationBuilder.DropTable(
                name: "BilliardTypeArea");

            migrationBuilder.DropIndex(
                name: "IX_Booking_AreaId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Booking");
        }
    }
}
