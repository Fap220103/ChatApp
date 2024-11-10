using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp_Api.Migrations
{
    public partial class addGroupChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupChats",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChats", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_GroupChats_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMembersChat",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMembersChat", x => new { x.GroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_GroupMembersChat_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GroupMembersChat_GroupChats_GroupId",
                        column: x => x.GroupId,
                        principalTable: "GroupChats",
                        principalColumn: "GroupId");
                });

            migrationBuilder.CreateTable(
                name: "MessagesChat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    SenderUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageSent = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesChat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessagesChat_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessagesChat_GroupChats_GroupId",
                        column: x => x.GroupId,
                        principalTable: "GroupChats",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChats_CreatedByUserId",
                table: "GroupChats",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembersChat_UserId",
                table: "GroupMembersChat",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesChat_GroupId",
                table: "MessagesChat",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MessagesChat_SenderId",
                table: "MessagesChat",
                column: "SenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupMembersChat");

            migrationBuilder.DropTable(
                name: "MessagesChat");

            migrationBuilder.DropTable(
                name: "GroupChats");
        }
    }
}
