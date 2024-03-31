using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance_Tracking_System.Migrations
{
    /// <inheritdoc />
    public partial class addsup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SuperID",
                table: "Track",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Track_SuperID",
                table: "Track",
                column: "SuperID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Track_Instructor_SuperID",
                table: "Track",
                column: "SuperID",
                principalTable: "Instructor",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Track_Instructor_SuperID",
                table: "Track");

            migrationBuilder.DropIndex(
                name: "IX_Track_SuperID",
                table: "Track");

            migrationBuilder.DropColumn(
                name: "SuperID",
                table: "Track");
        }
    }
}
