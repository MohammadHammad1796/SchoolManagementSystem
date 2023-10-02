using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authorization.Migrations;

public partial class HashRefreshToken : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "IX_RefreshTokens_Token",
			table: "RefreshTokens");

		migrationBuilder.RenameColumn(
			name: "Token",
			table: "RefreshTokens",
			newName: "TokenHash");

		migrationBuilder.AddColumn<string>(
			name: "Salt",
			table: "RefreshTokens",
			type: "nvarchar(450)",
			nullable: true);

		migrationBuilder.CreateIndex(
			name: "IX_RefreshTokens_Salt",
			table: "RefreshTokens",
			column: "Salt",
			unique: true,
			filter: "[Salt] IS NOT NULL");

		migrationBuilder.CreateIndex(
			name: "IX_RefreshTokens_TokenHash",
			table: "RefreshTokens",
			column: "TokenHash",
			unique: true,
			filter: "[TokenHash] IS NOT NULL");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "IX_RefreshTokens_Salt",
			table: "RefreshTokens");

		migrationBuilder.DropIndex(
			name: "IX_RefreshTokens_TokenHash",
			table: "RefreshTokens");

		migrationBuilder.DropColumn(
			name: "Salt",
			table: "RefreshTokens");

		migrationBuilder.RenameColumn(
			name: "TokenHash",
			table: "RefreshTokens",
			newName: "Token");

		migrationBuilder.CreateIndex(
			name: "IX_RefreshTokens_Token",
			table: "RefreshTokens",
			column: "Token",
			unique: true,
			filter: "[Token] IS NOT NULL");
	}
}