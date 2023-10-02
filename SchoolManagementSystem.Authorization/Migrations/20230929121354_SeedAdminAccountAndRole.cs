using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Authorization.Migrations;

public partial class SeedAdminAccountAndRole : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.InsertData(
			table: "AspNetRoles",
			columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
			values: new object[,]
			{
				{ "55d8b8fa-5eb1-4069-a19c-6967695c97ab", "3266ad79-95bf-440d-b98e-dc000478da9b", "Admin", "ADMIN" },
				{ "c06b3dc8-a22c-4f4d-955a-e2b2cd60dac0", "f952ed62-1324-41dd-8184-17bff7e171d7", "Student", "STUDENT" }
			});

		// admin password is Te$t4You
		migrationBuilder.InsertData(
			table: "AspNetUsers",
			columns: new[] { "Id", "AccessFailedCount", "Email", "EmailConfirmed", "LockoutEnabled",
				"NormalizedEmail", "NormalizedUserName",
				"PasswordHash",
				"PhoneNumberConfirmed", "UserName", "TwoFactorEnabled", "SecurityStamp",
				"ConcurrencyStamp" },
			values: new object[,]
			{
				{
					"db71636d-8201-42b9-8aca-d25101a179a4", 0, "Admin@SchoolManagementSystem.com", true, false,
					"ADMIN@SCHOOLMANAGEMENTSYSTEM.COM", "ADMIN@SCHOOLMANAGEMENTSYSTEM.COM",
					"AQAAAAEAACcQAAAAEHbbLf3EfIbRVyCHVBtqn1WXBrFa1EJcTIWhds7PDDl7k2w66+71GA1ow1lavr3F1Q==",
					false, "Admin@SchoolManagementSystem.com", false, "BEIWUPUXYVSNISIHCIJVH7IJBTOYEA4T",
					"1e6a4f38-1387-4472-82ef-1fa8ab6e99f2"
				},
			});

		migrationBuilder.InsertData(
			table: "AspNetUserRoles",
			columns: new[] { "UserId", "RoleId" },
			values: new object[,]
			{
				{
					"db71636d-8201-42b9-8aca-d25101a179a4", "55d8b8fa-5eb1-4069-a19c-6967695c97ab"
				}
			});
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql("DELETE FROM AspNetUserRoles");

		migrationBuilder.Sql("DELETE FROM AspNetUsers");

		migrationBuilder.Sql("DELETE FROM AspNetRoles");
	}
}