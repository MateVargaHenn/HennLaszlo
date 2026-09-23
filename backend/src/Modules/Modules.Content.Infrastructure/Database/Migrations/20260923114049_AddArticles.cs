using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Content.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddArticles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TitleHu = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    TitleEn = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SummaryHu = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SummaryEn = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ContentHu = table.Column<string>(type: "text", nullable: false),
                    ContentEn = table.Column<string>(type: "text", nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_IsPublished_DisplayOrder",
                schema: "content",
                table: "Articles",
                columns: new[] { "IsPublished", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Slug",
                schema: "content",
                table: "Articles",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles",
                schema: "content");
        }
    }
}
