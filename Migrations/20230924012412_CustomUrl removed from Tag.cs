using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Horizontal.Migrations
{
    public partial class CustomUrlremovedfromTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomUrl",
                table: "Tags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomUrl",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
