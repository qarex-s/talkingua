using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkingUADev.Data.Migrations
{
    public partial class addingCommentsText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ToPostId",
                table: "commentsUsers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "TextMessage",
                table: "commentsUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextMessage",
                table: "commentsUsers");

            migrationBuilder.AlterColumn<string>(
                name: "ToPostId",
                table: "commentsUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
