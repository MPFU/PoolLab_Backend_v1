using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRegisCourse5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCourse_StudentID",
                table: "RegisteredCourse",
                column: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredCourse_Student",
                table: "RegisteredCourse",
                column: "StudentID",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredCourse_Student",
                table: "RegisteredCourse");

            migrationBuilder.DropIndex(
                name: "IX_RegisteredCourse_StudentID",
                table: "RegisteredCourse");
        }
    }
}
