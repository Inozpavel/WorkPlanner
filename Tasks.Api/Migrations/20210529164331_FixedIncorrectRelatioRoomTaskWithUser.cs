using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks.Api.Migrations
{
    public partial class FixedIncorrectRelatioRoomTaskWithUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomTasks_UserInTheRoom_TaskCreatorRoomId_TaskCreatorUserId",
                table: "RoomTasks");

            migrationBuilder.DropIndex(
                name: "IX_RoomTasks_TaskCreatorRoomId_TaskCreatorUserId",
                table: "RoomTasks");

            migrationBuilder.DropColumn(
                name: "TaskCreatorRoomId",
                table: "RoomTasks");

            migrationBuilder.DropColumn(
                name: "TaskCreatorUserId",
                table: "RoomTasks");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTasks_TaskCreatorId",
                table: "RoomTasks",
                column: "TaskCreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTasks_Users_TaskCreatorId",
                table: "RoomTasks",
                column: "TaskCreatorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomTasks_Users_TaskCreatorId",
                table: "RoomTasks");

            migrationBuilder.DropIndex(
                name: "IX_RoomTasks_TaskCreatorId",
                table: "RoomTasks");

            migrationBuilder.AddColumn<Guid>(
                name: "TaskCreatorRoomId",
                table: "RoomTasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TaskCreatorUserId",
                table: "RoomTasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomTasks_TaskCreatorRoomId_TaskCreatorUserId",
                table: "RoomTasks",
                columns: new[] { "TaskCreatorRoomId", "TaskCreatorUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoomTasks_UserInTheRoom_TaskCreatorRoomId_TaskCreatorUserId",
                table: "RoomTasks",
                columns: new[] { "TaskCreatorRoomId", "TaskCreatorUserId" },
                principalTable: "UserInTheRoom",
                principalColumns: new[] { "RoomId", "UserId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
