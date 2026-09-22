using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Artwork.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "artwork");

            migrationBuilder.CreateTable(
                name: "Artworks",
                schema: "artwork",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TitleHu = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    TitleEn = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: true),
                    TechniqueHu = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TechniqueEn = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    WidthCm = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: true),
                    HeightCm = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: true),
                    DescriptionHu = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    DescriptionEn = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    ImageId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artworks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artworks_IsPublished",
                schema: "artwork",
                table: "Artworks",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Artworks_IsPublished_DisplayOrder",
                schema: "artwork",
                table: "Artworks",
                columns: new[] { "IsPublished", "DisplayOrder" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artworks",
                schema: "artwork");
        }
    }
}
