using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTCClassSupport.API.Migrations
{
    public partial class updateNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ObjectId",
                table: "notifications",
                type: "text",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObjectId",
                table: "notifications");
        }
    }
}
