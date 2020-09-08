using Microsoft.EntityFrameworkCore.Migrations;

namespace MySchool.Migrations
{
    public partial class SeedMenus1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Rights
            migrationBuilder.Sql("SET IDENTITY_INSERT [mRights] ON ");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (53, 7, N'Messages', N'Messages', N'/messages', 1, 40, 1, N'57', NULL)");
            migrationBuilder.Sql("INSERT [mRights] ([id], [menuid], [displayname], [rname], [url], [visible], [sort], [groupid], [authid], [parentid]) VALUES (54, 7, N'Message Details', N'Message Details', N'/messagedetails/0', 0, 41, 1, N'58', NULL)");
            migrationBuilder.Sql("SET IDENTITY_INSERT [mRights] OFF");


            //Role Rights
            migrationBuilder.Sql("SET IDENTITY_INSERT [mRoleRights] ON ");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (67, 1, 53)");
            migrationBuilder.Sql("INSERT [mRoleRights] ([id], [role_id], [right_id]) VALUES (68, 1, 54)");
            migrationBuilder.Sql("SET IDENTITY_INSERT [mRoleRights] OFF");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
