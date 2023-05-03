using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkingUADev.Data.Migrations
{
    public partial class initMigrationForStories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "listUserStories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listUserStories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_listUserStories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageStory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountWathcedStory = table.Column<int>(type: "int", nullable: false),
                    DateOfCreatingStory = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ListUserStoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stories_listUserStories_ListUserStoryId",
                        column: x => x.ListUserStoryId,
                        principalTable: "listUserStories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_listUserStories_UserId",
                table: "listUserStories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_stories_ListUserStoryId",
                table: "stories",
                column: "ListUserStoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stories");

            migrationBuilder.DropTable(
                name: "listUserStories");
        }
    }
}
