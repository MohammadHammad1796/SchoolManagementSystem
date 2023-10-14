using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolManagementSystem.Courses.Migrations;

public partial class InitialMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "Courses",
			columns: table => new
			{
				Id = table.Column<int>(type: "int", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
				SpecializeId = table.Column<int>(type: "int", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Courses", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "StudentCourses",
			columns: table => new
			{
				StudentId = table.Column<int>(type: "int", nullable: false),
				CourseId = table.Column<int>(type: "int", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_StudentCourses", x => new { x.CourseId, x.StudentId });
			});

		migrationBuilder.CreateIndex(
			name: "IX_Courses_Name_SpecializeId",
			table: "Courses",
			columns: new[] { "Name", "SpecializeId" },
			unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "Courses");

		migrationBuilder.DropTable(
			name: "StudentCourses");
	}
}