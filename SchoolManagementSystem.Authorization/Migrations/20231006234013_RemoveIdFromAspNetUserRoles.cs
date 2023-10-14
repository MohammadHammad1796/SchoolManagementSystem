using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Authorization.Migrations;

public partial class RemoveIdFromAspNetUserRoles : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "Id",
			table: "AspNetUserRoles");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<string>(
			name: "Id",
			table: "AspNetUserRoles",
			type: "nvarchar(max)",
			nullable: true);
	}
}