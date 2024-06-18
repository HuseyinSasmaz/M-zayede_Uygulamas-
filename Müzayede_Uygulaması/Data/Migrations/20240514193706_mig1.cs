using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Müzayede_Uygulaması.Data.Migrations
{
    public partial class mig1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ÜrünListelemes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Başlık = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Açıklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fiyat = table.Column<double>(type: "float", nullable: false),
                    ResimYolu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Satıldımı = table.Column<bool>(type: "bit", nullable: false),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ÜrünListelemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ÜrünListelemes_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teklifs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fiyat = table.Column<double>(type: "float", nullable: false),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ÜrünListelemeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teklifs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teklifs_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teklifs_ÜrünListelemes_ÜrünListelemeId",
                        column: x => x.ÜrünListelemeId,
                        principalTable: "ÜrünListelemes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Yorums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    İçerik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ÜrünListelemeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yorums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Yorums_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Yorums_ÜrünListelemes_ÜrünListelemeId",
                        column: x => x.ÜrünListelemeId,
                        principalTable: "ÜrünListelemes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teklifs_IdentityUserId",
                table: "Teklifs",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Teklifs_ÜrünListelemeId",
                table: "Teklifs",
                column: "ÜrünListelemeId");

            migrationBuilder.CreateIndex(
                name: "IX_ÜrünListelemes_IdentityUserId",
                table: "ÜrünListelemes",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Yorums_IdentityUserId",
                table: "Yorums",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Yorums_ÜrünListelemeId",
                table: "Yorums",
                column: "ÜrünListelemeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teklifs");

            migrationBuilder.DropTable(
                name: "Yorums");

            migrationBuilder.DropTable(
                name: "ÜrünListelemes");
        }
    }
}
