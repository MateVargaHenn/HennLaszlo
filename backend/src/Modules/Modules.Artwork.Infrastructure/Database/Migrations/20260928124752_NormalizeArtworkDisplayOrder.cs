using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Artwork.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeArtworkDisplayOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(
            MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                WITH ordered AS
                (
                    SELECT
                        "Id",
                        (
                            ROW_NUMBER() OVER
                            (
                                ORDER BY
                                    "DisplayOrder",
                                    "CreatedAtUtc" DESC,
                                    "Id"
                            ) - 1
                        )::integer AS "NewDisplayOrder"
                    FROM artwork."Artworks"
                )
                UPDATE artwork."Artworks" AS artwork
                SET "DisplayOrder" =
                    ordered."NewDisplayOrder"
                FROM ordered
                WHERE artwork."Id" = ordered."Id";
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
