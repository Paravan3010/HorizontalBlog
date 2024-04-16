using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Horizontal.Migrations
{
    public partial class CustomURLsstructuresadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomUrls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomUrl = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OriginalUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUrls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomUrlQueryValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomUrlMappingId = table.Column<int>(type: "int", nullable: false),
                    QueryKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QueryValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUrlQueryValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomUrlQueryValues_CustomUrls_CustomUrlMappingId",
                        column: x => x.CustomUrlMappingId,
                        principalTable: "CustomUrls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomUrlQueryValues_CustomUrlMappingId",
                table: "CustomUrlQueryValues",
                column: "CustomUrlMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomUrls_CustomUrl",
                table: "CustomUrls",
                column: "CustomUrl",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomUrlQueryValues");

            migrationBuilder.DropTable(
                name: "CustomUrls");
        }
    }
}
