using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConfigBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableAvailability");

            migrationBuilder.DropTable(
                name: "RecurringBookings");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "TimeStart",
                table: "Booking",
                type: "time(0)",
                precision: 0,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "TimeEnd",
                table: "Booking",
                type: "time(0)",
                precision: 0,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "BookingDate",
                table: "Booking",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEnd",
                table: "Booking",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateStart",
                table: "Booking",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                table: "Booking",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConfigTable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TimeDelay = table.Column<int>(type: "int", nullable: true),
                    TimeHold = table.Column<int>(type: "int", nullable: true),
                    Deposit = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigTable", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigTable");

            migrationBuilder.DropColumn(
                name: "BookingDate",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "DateEnd",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "DateStart",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Booking");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeStart",
                table: "Booking",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time(0)",
                oldPrecision: 0,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeEnd",
                table: "Booking",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time(0)",
                oldPrecision: 0,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RecurringBookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BilliardTableID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BilliardTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoreID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DaysOfWeek = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecBook_Account",
                        column: x => x.CustomerID,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecBook_BilliardTable",
                        column: x => x.BilliardTableID,
                        principalTable: "BilliardTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecBook_BilliardType",
                        column: x => x.BilliardTypeID,
                        principalTable: "BilliardType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecBook_Store",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TableAvailability",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BilliardTableID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecurringBookingID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableAvailability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableAvailability_BilliardTable",
                        column: x => x.BilliardTableID,
                        principalTable: "BilliardTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TableAvailability_Booking",
                        column: x => x.BookingID,
                        principalTable: "Booking",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TableAvailability_RecurringBooking",
                        column: x => x.RecurringBookingID,
                        principalTable: "RecurringBookings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecurringBookings_BilliardTableID",
                table: "RecurringBookings",
                column: "BilliardTableID");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringBookings_BilliardTypeID",
                table: "RecurringBookings",
                column: "BilliardTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringBookings_CustomerID",
                table: "RecurringBookings",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringBookings_StoreID",
                table: "RecurringBookings",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_TableAvailability_BilliardTableID",
                table: "TableAvailability",
                column: "BilliardTableID");

            migrationBuilder.CreateIndex(
                name: "IX_TableAvailability_BookingID",
                table: "TableAvailability",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_TableAvailability_RecurringBookingID",
                table: "TableAvailability",
                column: "RecurringBookingID");
        }
    }
}
