using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTCClassSupport.API.Migrations
{
    public partial class addFieldGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "allow_out",
                table: "groups",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "allow_out",
                table: "groups");
        }
    }
}
