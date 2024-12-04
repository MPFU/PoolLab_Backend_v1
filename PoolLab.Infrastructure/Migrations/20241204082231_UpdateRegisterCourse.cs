using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRegisterCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "CourseDate",
                table: "RegisteredCourse",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "RegisteredCourse",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "RegisteredCourse",
                type: "time(0)",
                precision: 0,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRegistered",
                table: "RegisteredCourse",
                type: "BIT",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "RegisteredCourse",
                type: "decimal(11,0)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RegisterCourseId",
                table: "RegisteredCourse",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Schedule",
                table: "RegisteredCourse",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "RegisteredCourse",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "RegisteredCourse",
                type: "time(0)",
                precision: 0,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCourse_RegisterCourseId",
                table: "RegisteredCourse",
                column: "RegisterCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredCourse_EnrollCourse",
                table: "RegisteredCourse",
                column: "RegisterCourseId",
                principalTable: "RegisteredCourse",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredCourse_EnrollCourse",
                table: "RegisteredCourse");

            migrationBuilder.DropIndex(
                name: "IX_RegisteredCourse_RegisterCourseId",
                table: "RegisteredCourse");

            migrationBuilder.DropColumn(
                name: "CourseDate",
                table: "RegisteredCourse");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "RegisteredCourse");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "RegisteredCourse");

            migrationBuilder.DropColumn(
                name: "IsRegistered",
                table: "RegisteredCourse");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "RegisteredCourse");

            migrationBuilder.DropColumn(
                name: "RegisterCourseId",
                table: "RegisteredCourse");

            migrationBuilder.DropColumn(
                name: "Schedule",
                table: "RegisteredCourse");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "RegisteredCourse");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "RegisteredCourse");
        }
    }
}
