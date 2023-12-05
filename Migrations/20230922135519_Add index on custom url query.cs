using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Horizontal.Migrations
{
    public partial class Addindexoncustomurlquery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustomUrl",
                table: "CustomUrlQueryValues",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomUrlQueryValues_CustomUrl",
                table: "CustomUrlQueryValues",
                column: "CustomUrl",
                unique: true,
                filter: "[CustomUrl] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomUrlQueryValues_CustomUrl",
                table: "CustomUrlQueryValues");

            migrationBuilder.AlterColumn<string>(
                name: "CustomUrl",
                table: "CustomUrlQueryValues",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
