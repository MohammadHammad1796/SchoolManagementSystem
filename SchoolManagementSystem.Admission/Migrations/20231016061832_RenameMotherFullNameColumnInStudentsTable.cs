using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Admission.Migrations;

public partial class RenameMotherFullNameColumnInStudentsTable : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.RenameColumn(
			name: "MotherFullname",
			table: "Students",
			newName: "MotherFullName");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.RenameColumn(
			name: "MotherFullName",
			table: "Students",
			newName: "MotherFullname");
	}
}