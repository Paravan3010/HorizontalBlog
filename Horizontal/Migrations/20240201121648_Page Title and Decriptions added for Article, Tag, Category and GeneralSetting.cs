using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Horizontal.Migrations
{
    public partial class PageTitleandDecriptionsaddedforArticleTagCategoryandGeneralSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PageDescription",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageTitle",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainPageDescription",
                table: "GeneralSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainPageTitle",
                table: "GeneralSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageDescription",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageTitle",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageDescription",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageTitle",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageDescription",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "PageTitle",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "MainPageDescription",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "MainPageTitle",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "PageDescription",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "PageTitle",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "PageDescription",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "PageTitle",
                table: "Articles");
        }
    }
}
