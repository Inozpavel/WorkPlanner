using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks.Api.Migrations
{
    public partial class AddedTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomTasks",
                columns: table => new
                {
                    RoomTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskName = table.Column<string>(type: "text", nullable: false),
                    TaskContent = table.Column<string>(type: "text", nullable: false),
                    Details = table.Column<string>(type: "text", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskCreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TaskCreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskCreatorRoomId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaskCreatorUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeadlineTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTasks", x => x.RoomTaskId);
                    table.ForeignKey(
                        name: "FK_RoomTasks_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomTasks_UsersInRoom_TaskCreatorRoomId_TaskCreatorUserId",
                        columns: x => new { x.TaskCreatorRoomId, x.TaskCreatorUserId },
                        principalTable: "UsersInRoom",
                        principalColumns: new[] { "RoomId", "UserId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomTasks_RoomId",
                table: "RoomTasks",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTasks_TaskCreatorRoomId_TaskCreatorUserId",
                table: "RoomTasks",
                columns: new[] { "TaskCreatorRoomId", "TaskCreatorUserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomTasks");
        }
    }
}
