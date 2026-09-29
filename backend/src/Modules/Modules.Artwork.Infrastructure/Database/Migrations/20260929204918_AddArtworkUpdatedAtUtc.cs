using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Artwork.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddArtworkUpdatedAtUtc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                schema: "artwork",
                table: "Artworks",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                schema: "artwork",
                table: "Artworks");
        }
    }
}
