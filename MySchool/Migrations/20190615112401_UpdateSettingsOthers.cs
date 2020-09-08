using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchool.Migrations
{
    public partial class UpdateSettingsOthers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "smtphost",
                table: "SettingsOther",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "smtpbccid",
                table: "SettingsOther",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "smtpenablessl",
                table: "SettingsOther",
                nullable: false,
                defaultValue: false);

            //Seed data SettingsOther
            migrationBuilder.Sql("UPDATE [SettingsOther] SET [smtpbccid] = 'support@edu-net.ga', [smtpenablessl] = 'false', [smtpemailid]='noreply@edu-net.ga', [smtphost]='mail.edu-net.ga', [smtppassword]='yQ2VqMdZkxe4hG9gZCjlWw==', [smtpport]=587 WHERE [id] = 1");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "smtpbccid",
                table: "SettingsOther");

            migrationBuilder.DropColumn(
                name: "smtpenablessl",
                table: "SettingsOther");

            migrationBuilder.AlterColumn<string>(
                name: "smtphost",
                table: "SettingsOther",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }
    }
}
