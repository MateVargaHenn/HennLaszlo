using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Invitation.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddExhibitionPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExhibitionEndsAt",
                schema: "invitation",
                table: "Invitations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExhibitionStartsAt",
                schema: "invitation",
                table: "Invitations",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExhibitionEndsAt",
                schema: "invitation",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "ExhibitionStartsAt",
                schema: "invitation",
                table: "Invitations");
        }
    }
}
