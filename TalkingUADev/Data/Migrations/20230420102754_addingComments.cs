using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalkingUADev.Data.Migrations
{
    public partial class addingComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "commentsUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToPostId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPostId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commentsUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_commentsUsers_Posts_UserPostId",
                        column: x => x.UserPostId,
                        principalTable: "Posts",
                        principalColumn: "UserPostId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_commentsUsers_UserPostId",
                table: "commentsUsers",
                column: "UserPostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "commentsUsers");
        }
    }
}
