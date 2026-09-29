using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Content.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddWebsiteSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebsiteSettings",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArtistName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ArtistSubtitle = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    HeroDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DefaultSeoTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DefaultSeoDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FacebookUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    InstagramUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    YoutubeUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "content",
                table: "WebsiteSettings",
                columns: new[] { "Id", "ArtistName", "ArtistSubtitle", "DefaultSeoDescription", "DefaultSeoTitle", "FacebookUrl", "HeroDescription", "InstagramUrl", "UpdatedAtUtc", "YoutubeUrl" },
                values: new object[] { new Guid("3d932c73-5380-4fb9-ae2f-394011659d73"), "Henn László András", "Galyasi Miklós nívódíjas festőművész, grafikus", "Henn László András Galyasi Miklós nívódíjas festőművész és grafikus hivatalos oldala: művek, kiállítások, meghívók, videók és írások.", "Henn László András | Festőművész és grafikus", null, "Válogatás az alkotó festményeiből, kiállításaiból és több évtizedes művészi munkásságából.", null, new DateTime(2026, 9, 29, 0, 0, 0, 0, DateTimeKind.Utc), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebsiteSettings",
                schema: "content");
        }
    }
}
