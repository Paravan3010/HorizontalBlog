using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Horizontal.Migrations
{
    public partial class Numberofvisitsaddedonarticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfVisits",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfVisits",
                table: "Articles");
        }
    }
}
