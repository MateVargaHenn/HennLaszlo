using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Video.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddVideoUpdatedAtUtc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                schema: "video",
                table: "Videos",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                schema: "video",
                table: "Videos");
        }
    }
}
