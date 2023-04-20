using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkingUADev.Data.Migrations
{
    public partial class changeForeignKeyForPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_commentsUsers_Posts_UserPostId",
                table: "commentsUsers");

            migrationBuilder.DropIndex(
                name: "IX_commentsUsers_UserPostId",
                table: "commentsUsers");

            migrationBuilder.DropColumn(
                name: "UserPostId",
                table: "commentsUsers");

            migrationBuilder.CreateIndex(
                name: "IX_commentsUsers_ToPostId",
                table: "commentsUsers",
                column: "ToPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_commentsUsers_Posts_ToPostId",
                table: "commentsUsers",
                column: "ToPostId",
                principalTable: "Posts",
                principalColumn: "UserPostId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_commentsUsers_Posts_ToPostId",
                table: "commentsUsers");

            migrationBuilder.DropIndex(
                name: "IX_commentsUsers_ToPostId",
                table: "commentsUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "UserPostId",
                table: "commentsUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_commentsUsers_UserPostId",
                table: "commentsUsers",
                column: "UserPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_commentsUsers_Posts_UserPostId",
                table: "commentsUsers",
                column: "UserPostId",
                principalTable: "Posts",
                principalColumn: "UserPostId");
        }
    }
}
