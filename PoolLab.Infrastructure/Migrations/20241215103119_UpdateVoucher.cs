using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TypeCode",
                table: "Voucher",
                newName: "Discount");

            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "AccountVoucher",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "AccountVoucher");

            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "Voucher",
                newName: "TypeCode");
        }
    }
}
