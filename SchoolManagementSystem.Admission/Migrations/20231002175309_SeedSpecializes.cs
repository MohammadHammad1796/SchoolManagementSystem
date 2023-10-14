using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Admission.Migrations;

public partial class SeedSpecializes : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.InsertData(
			table: "Specializes",
			columns: new[] { "Id", "Name" },
			values: new object[,]
			{
				{ 1, "Scientific" },
				{ 2, "Literary" },
				{ 3, "Legality" },
				{ 4, "Telecommunication" },
				{ 5, "Industry" },
				{ 6, "Art" },
				{ 7, "Informatics" }
			});
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql("DELETE FROM Specializes");
	}
}