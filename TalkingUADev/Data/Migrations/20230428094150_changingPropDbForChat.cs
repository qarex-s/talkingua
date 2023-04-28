using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkingUADev.Data.Migrations
{
    public partial class changingPropDbForChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chats_messages_MessageId",
                table: "chats");

            migrationBuilder.DropIndex(
                name: "IX_chats_MessageId",
                table: "chats");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "chats");

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_messages_ChatId",
                table: "messages",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_chats_ChatId",
                table: "messages",
                column: "ChatId",
                principalTable: "chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_chats_ChatId",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "IX_messages_ChatId",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "messages");

            migrationBuilder.AddColumn<int>(
                name: "MessageId",
                table: "chats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_chats_MessageId",
                table: "chats",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_chats_messages_MessageId",
                table: "chats",
                column: "MessageId",
                principalTable: "messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
