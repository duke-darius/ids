using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iceni.Lib.Migrations
{
    /// <inheritdoc />
    public partial class AddedLessonTypeToLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_AspNetUsers_TutorId",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_Pupils_PupilId",
                table: "Lesson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lesson",
                table: "Lesson");

            migrationBuilder.RenameTable(
                name: "Lesson",
                newName: "Lessons");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_TutorId",
                table: "Lessons",
                newName: "IX_Lessons_TutorId");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_PupilId",
                table: "Lessons",
                newName: "IX_Lessons_PupilId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lessons",
                table: "Lessons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_AspNetUsers_TutorId",
                table: "Lessons",
                column: "TutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Pupils_PupilId",
                table: "Lessons",
                column: "PupilId",
                principalTable: "Pupils",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_AspNetUsers_TutorId",
                table: "Lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Pupils_PupilId",
                table: "Lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lessons",
                table: "Lessons");

            migrationBuilder.RenameTable(
                name: "Lessons",
                newName: "Lesson");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_TutorId",
                table: "Lesson",
                newName: "IX_Lesson_TutorId");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_PupilId",
                table: "Lesson",
                newName: "IX_Lesson_PupilId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lesson",
                table: "Lesson",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_AspNetUsers_TutorId",
                table: "Lesson",
                column: "TutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_Pupils_PupilId",
                table: "Lesson",
                column: "PupilId",
                principalTable: "Pupils",
                principalColumn: "Id");
        }
    }
}
