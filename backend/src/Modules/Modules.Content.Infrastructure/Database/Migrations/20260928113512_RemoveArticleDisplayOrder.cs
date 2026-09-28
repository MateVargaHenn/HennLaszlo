using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Content.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveArticleDisplayOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_IsPublished_DisplayOrder",
                schema: "content",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                schema: "content",
                table: "Articles");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_IsPublished",
                schema: "content",
                table: "Articles",
                column: "IsPublished");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_IsPublished",
                schema: "content",
                table: "Articles");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                schema: "content",
                table: "Articles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_IsPublished_DisplayOrder",
                schema: "content",
                table: "Articles",
                columns: new[] { "IsPublished", "DisplayOrder" });
        }
    }
}
