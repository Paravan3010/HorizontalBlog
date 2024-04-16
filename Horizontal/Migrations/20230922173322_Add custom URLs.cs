using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Horizontal.Migrations
{
    public partial class AddcustomURLs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomUrls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewUrl = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OriginalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUrls", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomUrls_NewUrl",
                table: "CustomUrls",
                column: "NewUrl",
                unique: true,
                filter: "[NewUrl] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomUrls");
        }
    }
}
