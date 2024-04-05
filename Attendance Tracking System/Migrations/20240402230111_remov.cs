using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_Tracking_System.Migrations
{
    /// <inheritdoc />
    public partial class remov : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Intake_IntakeNo",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Program_ProgramID",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Track_TrackID",
                table: "Student");

            migrationBuilder.AlterColumn<string>(
                name: "UserImage",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "TrackID",
                table: "Student",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProgramID",
                table: "Student",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IntakeNo",
                table: "Student",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Intake_IntakeNo",
                table: "Student",
                column: "IntakeNo",
                principalTable: "Intake",
                principalColumn: "No");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Program_ProgramID",
                table: "Student",
                column: "ProgramID",
                principalTable: "Program",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Track_TrackID",
                table: "Student",
                column: "TrackID",
                principalTable: "Track",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Intake_IntakeNo",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Program_ProgramID",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Track_TrackID",
                table: "Student");

            migrationBuilder.AlterColumn<string>(
                name: "UserImage",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrackID",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProgramID",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IntakeNo",
                table: "Student",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Intake_IntakeNo",
                table: "Student",
                column: "IntakeNo",
                principalTable: "Intake",
                principalColumn: "No",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Program_ProgramID",
                table: "Student",
                column: "ProgramID",
                principalTable: "Program",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Track_TrackID",
                table: "Student",
                column: "TrackID",
                principalTable: "Track",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
