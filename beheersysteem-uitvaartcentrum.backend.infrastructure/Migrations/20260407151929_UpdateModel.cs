using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace beheersysteem_uitvaartcentrum.backend.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date_created",
                table: "Dossiers",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "Date_updated",
                table: "Documents",
                newName: "DateUpdated");

            migrationBuilder.RenameColumn(
                name: "Date_created",
                table: "Documents",
                newName: "DateCreated");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Dossiers",
                newName: "Date_created");

            migrationBuilder.RenameColumn(
                name: "DateUpdated",
                table: "Documents",
                newName: "Date_updated");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Documents",
                newName: "Date_created");
        }
    }
}
