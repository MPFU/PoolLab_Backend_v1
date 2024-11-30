using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountID",
                table: "Course",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoOfUser",
                table: "Course",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Schedule",
                table: "Course",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Course",
                type: "datetime",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Course_AccountID",
                table: "Course",
                column: "AccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Account",
                table: "Course",
                column: "AccountID",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Account",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_AccountID",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "AccountID",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "NoOfUser",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Schedule",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Course");
        }
    }
}
