using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkingUADev.Data.Migrations
{
    public partial class TestAddingUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userAppId",
                table: "commentsUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_commentsUsers_userAppId",
                table: "commentsUsers",
                column: "userAppId");

            migrationBuilder.AddForeignKey(
                name: "FK_commentsUsers_AspNetUsers_userAppId",
                table: "commentsUsers",
                column: "userAppId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_commentsUsers_AspNetUsers_userAppId",
                table: "commentsUsers");

            migrationBuilder.DropIndex(
                name: "IX_commentsUsers_userAppId",
                table: "commentsUsers");

            migrationBuilder.DropColumn(
                name: "userAppId",
                table: "commentsUsers");
        }
    }
}
