using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Content.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddContactContentPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    'f6b6c8e4-2054-4cbb-93fb-480c49a2d6ec'::uuid,
                    'contact',
                    'Kapcsolat',
                    'Contact',
                    '<p>Kapcsolatfelvételi adatok szerkesztés alatt.</p>',
                    '<p>Contact information is being edited.</p>',
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
                    WHERE "Id" =
                        'f6b6c8e4-2054-4cbb-93fb-480c49a2d6ec'::uuid;
                    """);
        }
    }
}
