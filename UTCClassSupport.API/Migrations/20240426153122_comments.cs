using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UTCClassSupport.API.Migrations
{
    public partial class comments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "timetables",
                newName: "group_id");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "groups",
                type: "varchar(95)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(95)",
                oldDefaultValueSql: "UUID()")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(95)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    post_id = table.Column<string>(type: "varchar(95)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_comments_bulletins_post_id",
                        column: x => x.post_id,
                        principalTable: "bulletins",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_comments_post_id",
                table: "comments",
                column: "post_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.RenameColumn(
                name: "group_id",
                table: "timetables",
                newName: "GroupId");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "groups",
                type: "varchar(95)",
                nullable: false,
                defaultValueSql: "UUID()",
                oldClrType: typeof(string),
                oldType: "varchar(95)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
