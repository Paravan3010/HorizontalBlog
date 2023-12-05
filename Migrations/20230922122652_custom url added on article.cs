using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Horizontal.Migrations
{
    public partial class customurladdedonarticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomUrl",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomUrl",
                table: "Articles");
        }
    }
}
