using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchool.Migrations
{
    public partial class SeedMenuRpt_S_Idnty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Rights
            migrationBuilder.Sql("SET IDENTITY_INSERT [mRights] ON ");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (55, 8, N'Student Identity Cards', N'Student Identity', N'/studentidcards', 1, 18, 15, N'59', NULL)");
            migrationBuilder.Sql("SET IDENTITY_INSERT [mRights] OFF");


            //Role Rights
            migrationBuilder.Sql("SET IDENTITY_INSERT [mRoleRights] ON ");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (69, 1, 55)");
            migrationBuilder.Sql("SET IDENTITY_INSERT [mRoleRights] OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
