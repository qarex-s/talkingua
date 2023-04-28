using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkingUADev.Data.Migrations
{
    public partial class changingPropForChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chatRooms_chats_ChatId",
                table: "chatRooms");

            migrationBuilder.DropIndex(
                name: "IX_chatRooms_ChatId",
                table: "chatRooms");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "chats");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "chatRooms");

            migrationBuilder.AddColumn<int>(
                name: "chatRoomId",
                table: "chats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_chats_chatRoomId",
                table: "chats",
                column: "chatRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_chats_chatRooms_chatRoomId",
                table: "chats",
                column: "chatRoomId",
                principalTable: "chatRooms",
                principalColumn: "ChatRoomId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chats_chatRooms_chatRoomId",
                table: "chats");

            migrationBuilder.DropIndex(
                name: "IX_chats_chatRoomId",
                table: "chats");

            migrationBuilder.DropColumn(
                name: "chatRoomId",
                table: "chats");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "chats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "chatRooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_chatRooms_ChatId",
                table: "chatRooms",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_chatRooms_chats_ChatId",
                table: "chatRooms",
                column: "ChatId",
                principalTable: "chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
