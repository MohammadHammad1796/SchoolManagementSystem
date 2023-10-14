using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Admission.Migrations;

public partial class AddReviewedColumnToStudentsTable : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<bool>(
			name: "Reviewed",
			table: "Students",
			type: "bit",
			nullable: false,
			defaultValue: false);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "Reviewed",
			table: "Students");
	}
}