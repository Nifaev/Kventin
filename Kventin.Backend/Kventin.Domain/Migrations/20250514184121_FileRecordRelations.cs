using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kventin.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FileRecordRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnouncementId",
                table: "FileRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExerciseAnswerId",
                table: "FileRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExerciseId",
                table: "FileRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LessonId",
                table: "FileRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MessageId",
                table: "FileRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NotificationId",
                table: "FileRecords",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileRecords_AnnouncementId",
                table: "FileRecords",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_FileRecords_ExerciseAnswerId",
                table: "FileRecords",
                column: "ExerciseAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_FileRecords_ExerciseId",
                table: "FileRecords",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_FileRecords_LessonId",
                table: "FileRecords",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_FileRecords_MessageId",
                table: "FileRecords",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_FileRecords_NotificationId",
                table: "FileRecords",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileRecords_Announcements_AnnouncementId",
                table: "FileRecords",
                column: "AnnouncementId",
                principalTable: "Announcements",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileRecords_ExerciseAnswers_ExerciseAnswerId",
                table: "FileRecords",
                column: "ExerciseAnswerId",
                principalTable: "ExerciseAnswers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileRecords_Exercises_ExerciseId",
                table: "FileRecords",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileRecords_Lessons_LessonId",
                table: "FileRecords",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileRecords_Messages_MessageId",
                table: "FileRecords",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileRecords_Notifications_NotificationId",
                table: "FileRecords",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileRecords_Announcements_AnnouncementId",
                table: "FileRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_FileRecords_ExerciseAnswers_ExerciseAnswerId",
                table: "FileRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_FileRecords_Exercises_ExerciseId",
                table: "FileRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_FileRecords_Lessons_LessonId",
                table: "FileRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_FileRecords_Messages_MessageId",
                table: "FileRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_FileRecords_Notifications_NotificationId",
                table: "FileRecords");

            migrationBuilder.DropIndex(
                name: "IX_FileRecords_AnnouncementId",
                table: "FileRecords");

            migrationBuilder.DropIndex(
                name: "IX_FileRecords_ExerciseAnswerId",
                table: "FileRecords");

            migrationBuilder.DropIndex(
                name: "IX_FileRecords_ExerciseId",
                table: "FileRecords");

            migrationBuilder.DropIndex(
                name: "IX_FileRecords_LessonId",
                table: "FileRecords");

            migrationBuilder.DropIndex(
                name: "IX_FileRecords_MessageId",
                table: "FileRecords");

            migrationBuilder.DropIndex(
                name: "IX_FileRecords_NotificationId",
                table: "FileRecords");

            migrationBuilder.DropColumn(
                name: "AnnouncementId",
                table: "FileRecords");

            migrationBuilder.DropColumn(
                name: "ExerciseAnswerId",
                table: "FileRecords");

            migrationBuilder.DropColumn(
                name: "ExerciseId",
                table: "FileRecords");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "FileRecords");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "FileRecords");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "FileRecords");
        }
    }
}
