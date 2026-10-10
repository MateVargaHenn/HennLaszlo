using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Invitation.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddInvitationLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationEn",
                schema: "invitation",
                table: "Invitations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationHu",
                schema: "invitation",
                table: "Invitations",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationEn",
                schema: "invitation",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "LocationHu",
                schema: "invitation",
                table: "Invitations");
        }
    }
}
