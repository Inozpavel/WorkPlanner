using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasks.Api.Migrations
{
    public partial class TaskCanNowBeCompleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "RoomTasks",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "RoomTasks");
        }
    }
}
