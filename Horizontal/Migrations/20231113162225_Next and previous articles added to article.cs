using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Horizontal.Migrations
{
    public partial class Nextandpreviousarticlesaddedtoarticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NextArticleId",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousArticleId",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_NextArticleId",
                table: "Articles",
                column: "NextArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_PreviousArticleId",
                table: "Articles",
                column: "PreviousArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Articles_NextArticleId",
                table: "Articles",
                column: "NextArticleId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Articles_PreviousArticleId",
                table: "Articles",
                column: "PreviousArticleId",
                principalTable: "Articles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Articles_NextArticleId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Articles_PreviousArticleId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_NextArticleId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_PreviousArticleId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "NextArticleId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "PreviousArticleId",
                table: "Articles");
        }
    }
}
