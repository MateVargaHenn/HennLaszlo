using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Content.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddWebsiteSettingsEmailAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                schema: "content",
                table: "WebsiteSettings",
                type: "character varying(254)",
                maxLength: 254,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "content",
                table: "WebsiteSettings",
                keyColumn: "Id",
                keyValue: new Guid("3d932c73-5380-4fb9-ae2f-394011659d73"),
                column: "EmailAddress",
                value: "hennlaa@gmail.com");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                schema: "content",
                table: "WebsiteSettings");
        }
    }
}
