using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkingUADev.Data.Migrations
{
    public partial class addingFlagMainUserForMEssage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainUserId",
                table: "messages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_messages_MainUserId",
                table: "messages",
                column: "MainUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_AspNetUsers_MainUserId",
                table: "messages",
                column: "MainUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_AspNetUsers_MainUserId",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "IX_messages_MainUserId",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "MainUserId",
                table: "messages");
        }
    }
}
