using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks.Api.Migrations
{
    public partial class UserNowCanHaveOnlyOneRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomRoleUserInRoom");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Rooms");

            migrationBuilder.AddColumn<Guid>(
                name: "RoomRoleId",
                table: "UsersInRoom",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UsersInRoom_RoomRoleId",
                table: "UsersInRoom",
                column: "RoomRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInRoom_RoomRoles_RoomRoleId",
                table: "UsersInRoom",
                column: "RoomRoleId",
                principalTable: "RoomRoles",
                principalColumn: "RoomRoleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersInRoom_RoomRoles_RoomRoleId",
                table: "UsersInRoom");

            migrationBuilder.DropIndex(
                name: "IX_UsersInRoom_RoomRoleId",
                table: "UsersInRoom");

            migrationBuilder.DropColumn(
                name: "RoomRoleId",
                table: "UsersInRoom");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Rooms",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "RoomRoleUserInRoom",
                columns: table => new
                {
                    RoomRolesRoomRoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserInRoomsRoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserInRoomsUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRoleUserInRoom", x => new { x.RoomRolesRoomRoleId, x.UserInRoomsRoomId, x.UserInRoomsUserId });
                    table.ForeignKey(
                        name: "FK_RoomRoleUserInRoom_RoomRoles_RoomRolesRoomRoleId",
                        column: x => x.RoomRolesRoomRoleId,
                        principalTable: "RoomRoles",
                        principalColumn: "RoomRoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomRoleUserInRoom_UsersInRoom_UserInRoomsRoomId_UserInRoom~",
                        columns: x => new { x.UserInRoomsRoomId, x.UserInRoomsUserId },
                        principalTable: "UsersInRoom",
                        principalColumns: new[] { "RoomId", "UserId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomRoleUserInRoom_UserInRoomsRoomId_UserInRoomsUserId",
                table: "RoomRoleUserInRoom",
                columns: new[] { "UserInRoomsRoomId", "UserInRoomsUserId" });
        }
    }
}
