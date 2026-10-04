using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Content.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddArticleVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Version",
                schema: "content",
                table: "Articles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
                
            migrationBuilder.Sql(
                """
                UPDATE content."Articles"
                SET "Version" = "Id";
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                schema: "content",
                table: "Articles");
        }
    }
}
