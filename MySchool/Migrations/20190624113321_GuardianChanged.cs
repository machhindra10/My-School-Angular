using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchool.Migrations
{
    public partial class GuardianChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "guardian_addresss",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "guardian_mobile",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "guardian_name",
                table: "Student");

            migrationBuilder.AddColumn<long>(
                name: "guardianid",
                table: "Student",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StudentGuardian",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 500, nullable: false),
                    mobile = table.Column<long>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    address = table.Column<string>(maxLength: 500, nullable: true),
                    disabled = table.Column<bool>(nullable: false),
                    userid = table.Column<int>(nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: true),
                    passverificationcode = table.Column<string>(maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGuardian", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_guardianid",
                table: "Student",
                column: "guardianid");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_StudentGuardian",
                table: "Student",
                column: "guardianid",
                principalTable: "StudentGuardian",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_StudentGuardian",
                table: "Student");

            migrationBuilder.DropTable(
                name: "StudentGuardian");

            migrationBuilder.DropIndex(
                name: "IX_Student_guardianid",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "guardianid",
                table: "Student");

            migrationBuilder.AddColumn<string>(
                name: "guardian_addresss",
                table: "Student",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "guardian_mobile",
                table: "Student",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "guardian_name",
                table: "Student",
                maxLength: 200,
                nullable: true);
        }
    }
}
