using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Horizontal.Migrations
{
    public partial class IsPublishedaddedonTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Tags",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Tags");
        }
    }
}
