using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BigUpdateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Order",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsRead = table.Column<bool>(type: "BIT", nullable: true, defaultValue: false),
                    ReadAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Customer",
                        column: x => x.CustomerID,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TableIssues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BilliardTableID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IssueImg = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Descript = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    EstimatedCost = table.Column<decimal>(type: "decimal(11,0)", nullable: true),
                    ReportedBy = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableIssues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountVouchers_BilliardTable",
                        column: x => x.BilliardTableID,
                        principalTable: "BilliardTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TableIssues_Customer",
                        column: x => x.CustomerID,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TableIssues_Store",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TableMaintenance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BilliardTableID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TableIssuesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TechnicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableMaintenance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableMaintenance_BilliardTable",
                        column: x => x.BilliardTableID,
                        principalTable: "BilliardTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TableMaintenance_Store",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TableMaintenance_TableIssues",
                        column: x => x.TableIssuesId,
                        principalTable: "TableIssues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TableMaintenance_Technician",
                        column: x => x.TechnicianId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_CustomerID",
                table: "Notification",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TableIssues_BilliardTableID",
                table: "TableIssues",
                column: "BilliardTableID");

            migrationBuilder.CreateIndex(
                name: "IX_TableIssues_CustomerID",
                table: "TableIssues",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_TableIssues_StoreId",
                table: "TableIssues",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TableMaintenance_BilliardTableID",
                table: "TableMaintenance",
                column: "BilliardTableID");

            migrationBuilder.CreateIndex(
                name: "IX_TableMaintenance_StoreId",
                table: "TableMaintenance",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TableMaintenance_TableIssuesId",
                table: "TableMaintenance",
                column: "TableIssuesId");

            migrationBuilder.CreateIndex(
                name: "IX_TableMaintenance_TechnicianId",
                table: "TableMaintenance",
                column: "TechnicianId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "TableMaintenance");

            migrationBuilder.DropTable(
                name: "TableIssues");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Order");
        }
    }
}
