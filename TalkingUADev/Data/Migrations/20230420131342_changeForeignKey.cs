using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkingUADev.Data.Migrations
{
    public partial class changeForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "FromUserId",
                table: "commentsUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_commentsUsers_FromUserId",
                table: "commentsUsers",
                column: "FromUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_commentsUsers_AspNetUsers_FromUserId",
                table: "commentsUsers",
                column: "FromUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_commentsUsers_AspNetUsers_FromUserId",
                table: "commentsUsers");

            migrationBuilder.DropIndex(
                name: "IX_commentsUsers_FromUserId",
                table: "commentsUsers");

            migrationBuilder.AlterColumn<string>(
                name: "FromUserId",
                table: "commentsUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
    }
}
