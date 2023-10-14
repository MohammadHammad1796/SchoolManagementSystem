using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Attendance.Migrations;

public partial class InitialMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "AttendanceDates",
			columns: table => new
			{
				Id = table.Column<int>(type: "int", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				Date = table.Column<DateTime>(type: "datetime2", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_AttendanceDates", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "StudentAttendances",
			columns: table => new
			{
				Id = table.Column<int>(type: "int", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				StudentId = table.Column<int>(type: "int", nullable: false),
				AttendanceDateId = table.Column<int>(type: "int", nullable: false),
				IsAttended = table.Column<bool>(type: "bit", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_StudentAttendances", x => x.Id);
				table.ForeignKey(
					name: "FK_StudentAttendances_AttendanceDates_AttendanceDateId",
					column: x => x.AttendanceDateId,
					principalTable: "AttendanceDates",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "IX_AttendanceDates_Date",
			table: "AttendanceDates",
			column: "Date",
			unique: true);

		migrationBuilder.CreateIndex(
			name: "IX_StudentAttendances_AttendanceDateId_StudentId",
			table: "StudentAttendances",
			columns: new[] { "AttendanceDateId", "StudentId" },
			unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "StudentAttendances");

		migrationBuilder.DropTable(
			name: "AttendanceDates");
	}
}