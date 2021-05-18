using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks.Api.Migrations
{
    public partial class AddedRegisteredUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomTasks_UsersInRoom_TaskCreatorRoomId_TaskCreatorUserId",
                table: "RoomTasks");

            migrationBuilder.DropTable(
                name: "UsersInRoom");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    Patronymic = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserInTheRoom",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomRoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInTheRoom", x => new { x.RoomId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserInTheRoom_RoomRoles_RoomRoleId",
                        column: x => x.RoomRoleId,
                        principalTable: "RoomRoles",
                        principalColumn: "RoomRoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInTheRoom_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInTheRoom_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInTheRoom_RoomRoleId",
                table: "UserInTheRoom",
                column: "RoomRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInTheRoom_UserId",
                table: "UserInTheRoom",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTasks_UserInTheRoom_TaskCreatorRoomId_TaskCreatorUserId",
                table: "RoomTasks",
                columns: new[] { "TaskCreatorRoomId", "TaskCreatorUserId" },
                principalTable: "UserInTheRoom",
                principalColumns: new[] { "RoomId", "UserId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomTasks_UserInTheRoom_TaskCreatorRoomId_TaskCreatorUserId",
                table: "RoomTasks");

            migrationBuilder.DropTable(
                name: "UserInTheRoom");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "UsersInRoom",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomRoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInRoom", x => new { x.RoomId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UsersInRoom_RoomRoles_RoomRoleId",
                        column: x => x.RoomRoleId,
                        principalTable: "RoomRoles",
                        principalColumn: "RoomRoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersInRoom_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersInRoom_RoomRoleId",
                table: "UsersInRoom",
                column: "RoomRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTasks_UsersInRoom_TaskCreatorRoomId_TaskCreatorUserId",
                table: "RoomTasks",
                columns: new[] { "TaskCreatorRoomId", "TaskCreatorUserId" },
                principalTable: "UsersInRoom",
                principalColumns: new[] { "RoomId", "UserId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
