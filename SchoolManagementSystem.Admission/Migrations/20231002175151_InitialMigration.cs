using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagementSystem.Admission.Migrations;

public partial class InitialMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "Specializes",
			columns: table => new
			{
				Id = table.Column<int>(type: "int", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Specializes", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "Students",
			columns: table => new
			{
				Id = table.Column<int>(type: "int", nullable: false)
					.Annotation("SqlServer:Identity", "1, 1"),
				FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
				LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
				MotherFullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
				FatherFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
				IsMale = table.Column<bool>(type: "bit", nullable: false),
				DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
				CertificateAverage = table.Column<double>(type: "float", nullable: false),
				CertificateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
				IsAccepted = table.Column<bool>(type: "bit", nullable: false),
				SpecializeId = table.Column<int>(type: "int", nullable: false),
				UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Students", x => x.Id);
				table.ForeignKey(
					name: "FK_Students_Specializes_SpecializeId",
					column: x => x.SpecializeId,
					principalTable: "Specializes",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "IX_Specializes_Name",
			table: "Specializes",
			column: "Name",
			unique: true);

		migrationBuilder.CreateIndex(
			name: "IX_Students_SpecializeId",
			table: "Students",
			column: "SpecializeId");

		migrationBuilder.CreateIndex(
			name: "IX_Students_UserId",
			table: "Students",
			column: "UserId",
			unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "Students");

		migrationBuilder.DropTable(
			name: "Specializes");
	}
}