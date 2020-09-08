using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchool.Migrations
{
    public partial class WebMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    message = table.Column<string>(nullable: false),
                    type = table.Column<string>(maxLength: 3, nullable: true),
                    datecreated = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    userid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MessagesGuardians",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    messageid = table.Column<long>(nullable: false),
                    guardianid = table.Column<long>(nullable: false),
                    delivered = table.Column<DateTime>(type: "datetime", nullable: true),
                    read = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesGuardians", x => x.id);
                    table.ForeignKey(
                        name: "FK_MessagesGuardians_StudentGuardian",
                        column: x => x.guardianid,
                        principalTable: "StudentGuardian",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageRecepients_Messages",
                        column: x => x.messageid,
                        principalTable: "Messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessagesStudents",
                columns: table => new
                {
                    id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    messageid = table.Column<long>(nullable: false),
                    studentid = table.Column<int>(nullable: false),
                    delivered = table.Column<DateTime>(type: "datetime", nullable: true),
                    read = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesStudents", x => x.id);
                    table.ForeignKey(
                        name: "FK_MessagesStudents_Messages",
                        column: x => x.messageid,
                        principalTable: "Messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessagesStudents_Student",
                        column: x => x.studentid,
                        principalTable: "Student",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessagesGuardians_guardianid",
                table: "MessagesGuardians",
                column: "guardianid");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesGuardians_messageid",
                table: "MessagesGuardians",
                column: "messageid");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesStudents_messageid",
                table: "MessagesStudents",
                column: "messageid");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesStudents_studentid",
                table: "MessagesStudents",
                column: "studentid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessagesGuardians");

            migrationBuilder.DropTable(
                name: "MessagesStudents");

            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
