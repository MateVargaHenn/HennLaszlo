using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Invitation.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeInvitationDisplayOrder : Migration
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
                    FROM invitation."Invitations"
                )
                UPDATE invitation."Invitations" AS invitation
                SET "DisplayOrder" =
                    ordered."NewDisplayOrder"
                FROM ordered
                WHERE invitation."Id" = ordered."Id";
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
