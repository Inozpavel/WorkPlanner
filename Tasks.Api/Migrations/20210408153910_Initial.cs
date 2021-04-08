using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks.Api.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "RoomRoles",
                table => new
                {
                    RoomRoleId = table.Column<Guid>("uuid", nullable: false),
                    RoomRoleName = table.Column<string>("text", nullable: false),
                    RoomRoleDescription = table.Column<string>("text", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_RoomRoles", x => x.RoomRoleId); });

            migrationBuilder.CreateTable(
                "Rooms",
                table => new
                {
                    RoomId = table.Column<Guid>("uuid", nullable: false),
                    OwnerId = table.Column<Guid>("uuid", nullable: false),
                    RoomName = table.Column<string>("text", nullable: false),
                    RoomDescription = table.Column<string>("text", nullable: true),
                    CreationDate = table.Column<DateTime>("timestamp without time zone", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Rooms", x => x.RoomId); });

            migrationBuilder.CreateTable(
                "UsersInRoom",
                table => new
                {
                    RoomId = table.Column<Guid>("uuid", nullable: false),
                    UserId = table.Column<Guid>("uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInRoom", x => new {x.RoomId, x.UserId});
                    table.ForeignKey(
                        "FK_UsersInRoom_Rooms_RoomId",
                        x => x.RoomId,
                        "Rooms",
                        "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "RoomRoleUserInRoom",
                table => new
                {
                    RoomRolesRoomRoleId = table.Column<Guid>("uuid", nullable: false),
                    UserInRoomsRoomId = table.Column<Guid>("uuid", nullable: false),
                    UserInRoomsUserId = table.Column<Guid>("uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRoleUserInRoom",
                        x => new {x.RoomRolesRoomRoleId, x.UserInRoomsRoomId, x.UserInRoomsUserId});
                    table.ForeignKey(
                        "FK_RoomRoleUserInRoom_RoomRoles_RoomRolesRoomRoleId",
                        x => x.RoomRolesRoomRoleId,
                        "RoomRoles",
                        "RoomRoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_RoomRoleUserInRoom_UsersInRoom_UserInRoomsRoomId_UserInRoom~",
                        x => new {x.UserInRoomsRoomId, x.UserInRoomsUserId},
                        "UsersInRoom",
                        new[] {"RoomId", "UserId"},
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_RoomRoleUserInRoom_UserInRoomsRoomId_UserInRoomsUserId",
                "RoomRoleUserInRoom",
                new[] {"UserInRoomsRoomId", "UserInRoomsUserId"});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "RoomRoleUserInRoom");

            migrationBuilder.DropTable(
                "RoomRoles");

            migrationBuilder.DropTable(
                "UsersInRoom");

            migrationBuilder.DropTable(
                "Rooms");
        }
    }
}