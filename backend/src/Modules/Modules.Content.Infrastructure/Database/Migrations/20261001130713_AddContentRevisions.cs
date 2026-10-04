using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Content.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddContentRevisions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContentRevisions",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetType = table.Column<int>(type: "integer", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    TitleHu = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    TitleEn = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SummaryHu = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SummaryEn = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ContentHu = table.Column<string>(type: "text", nullable: false),
                    ContentEn = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentRevisions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentRevisions_TargetType_TargetId_CreatedAtUtc",
                schema: "content",
                table: "ContentRevisions",
                columns: new[] { "TargetType", "TargetId", "CreatedAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentRevisions",
                schema: "content");
        }
    }
}
