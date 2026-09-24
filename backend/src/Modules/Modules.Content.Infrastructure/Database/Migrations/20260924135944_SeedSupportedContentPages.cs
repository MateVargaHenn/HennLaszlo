using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Content.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class SeedSupportedContentPages : Migration
    {
        /// <inheritdoc />
        protected override void Up(
            MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                INSERT INTO content."ContentPages"
                (
                    "Id",
                    "Key",
                    "TitleHu",
                    "TitleEn",
                    "ContentHu",
                    "ContentEn",
                    "IsPublished",
                    "CreatedAtUtc",
                    "UpdatedAtUtc"
                )
                VALUES
                (
                    '8b72c289-9dc5-4e50-a1b0-4fb78b16b6f2'::uuid,
                    'about',
                    'Bemutatkozás',
                    'About',
                    '<p>A tartalom szerkesztés alatt.</p>',
                    '<p>Content is being edited.</p>',
                    FALSE,
                    CURRENT_TIMESTAMP,
                    CURRENT_TIMESTAMP
                ),
                (
                    '0fd9fe5b-f929-4e93-8b0c-2826e7e24c3e'::uuid,
                    'exhibitions',
                    'Kiállítások',
                    'Exhibitions',
                    '<p>A tartalom szerkesztés alatt.</p>',
                    '<p>Content is being edited.</p>',
                    FALSE,
                    CURRENT_TIMESTAMP,
                    CURRENT_TIMESTAMP
                ),
                (
                    'a154f14e-75e4-4ca0-9b3c-d3b1c3f0222a'::uuid,
                    'memberships-and-awards',
                    'Tagságok és díjak',
                    'Memberships and awards',
                    '<p>A tartalom szerkesztés alatt.</p>',
                    '<p>Content is being edited.</p>',
                    FALSE,
                    CURRENT_TIMESTAMP,
                    CURRENT_TIMESTAMP
                ),
                (
                    'ca28a45c-0d9a-4d31-bb8d-20d88c5581d0'::uuid,
                    'writings',
                    'Írások',
                    'Writings',
                    '<p>A tartalom szerkesztés alatt.</p>',
                    '<p>Content is being edited.</p>',
                    FALSE,
                    CURRENT_TIMESTAMP,
                    CURRENT_TIMESTAMP
                )
                ON CONFLICT ("Key") DO NOTHING;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DELETE FROM content."ContentPages"
                WHERE "Id" IN
                (
                    '8b72c289-9dc5-4e50-a1b0-4fb78b16b6f2'::uuid,
                    '0fd9fe5b-f929-4e93-8b0c-2826e7e24c3e'::uuid,
                    'a154f14e-75e4-4ca0-9b3c-d3b1c3f0222a'::uuid,
                    'ca28a45c-0d9a-4d31-bb8d-20d88c5581d0'::uuid
                );
                """);
        }
    }
}
