using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PoolLab.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRegisCourse4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegisterCourseId",
                table: "RegisteredCourse",
                newName: "EnrollCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_RegisteredCourse_RegisterCourseId",
                table: "RegisteredCourse",
                newName: "IX_RegisteredCourse_EnrollCourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EnrollCourseId",
                table: "RegisteredCourse",
                newName: "RegisterCourseId");

            migrationBuilder.RenameIndex(
                name: "IX_RegisteredCourse_EnrollCourseId",
                table: "RegisteredCourse",
                newName: "IX_RegisteredCourse_RegisterCourseId");
        }
    }
}
