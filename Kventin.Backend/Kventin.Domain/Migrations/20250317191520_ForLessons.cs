using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kventin.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ForLessons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IndividualStudentId",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "Exercises",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_IndividualStudentId",
                table: "Exercises",
                column: "IndividualStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_LessonId",
                table: "Exercises",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Lessons_LessonId",
                table: "Exercises",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Users_IndividualStudentId",
                table: "Exercises",
                column: "IndividualStudentId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Lessons_LessonId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Users_IndividualStudentId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_IndividualStudentId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_LessonId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "IndividualStudentId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Exercises");
        }
    }
}
