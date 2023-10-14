using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Authorization.Migrations;

public partial class SeedNoRole : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.InsertData(
			table: "AspNetRoles",
			columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
			values: new object[,]
			{
				{ "4e9032cc-8fce-41f2-a9e3-5664cffb70f5", "6d3e4c48-7d66-4b36-8c45-2daf886ce612", "NoRole", "NOROLE" }
			});
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql("DELETE FROM AspNetRoles WHERE Id = '4e9032cc-8fce-41f2-a9e3-5664cffb70f5'");
	}
}