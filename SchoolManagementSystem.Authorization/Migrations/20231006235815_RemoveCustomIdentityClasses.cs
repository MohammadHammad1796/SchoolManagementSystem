﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Authorization.Migrations;

public partial class RemoveCustomIdentityClasses : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<string>(
			name: "Discriminator",
			table: "AspNetUsers",
			type: "nvarchar(max)",
			nullable: false,
			defaultValue: "");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "Discriminator",
			table: "AspNetUsers");
	}
}