using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Invitation.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "invitation");

            migrationBuilder.CreateTable(
                name: "Invitations",
                schema: "invitation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TitleHu = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    TitleEn = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: true),
                    AltTextHu = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AltTextEn = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ImageId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_IsPublished_DisplayOrder",
                schema: "invitation",
                table: "Invitations",
                columns: new[] { "IsPublished", "DisplayOrder" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitations",
                schema: "invitation");
        }
    }
}
