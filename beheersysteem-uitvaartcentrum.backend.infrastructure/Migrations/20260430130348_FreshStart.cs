using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace beheersysteem_uitvaartcentrum.backend.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FreshStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Dossiers_DossierId",
                table: "Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Dossiers_Id",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "DateUpdated",
                table: "Documents",
                newName: "DateUploaded");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Dossiers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Dossiers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "DossierId",
                table: "Documents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Documents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "DossierInvited",
                columns: table => new
                {
                    DossierId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DossierInvited", x => new { x.DossierId, x.UserId });
                    table.ForeignKey(
                        name: "FK_DossierInvited_Dossiers_DossierId",
                        column: x => x.DossierId,
                        principalTable: "Dossiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Dossiers_DossierId",
                table: "Documents",
                column: "DossierId",
                principalTable: "Dossiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Dossiers_DossierId",
                table: "Documents");

            migrationBuilder.DropTable(
                name: "DossierInvited");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Dossiers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "DateUploaded",
                table: "Documents",
                newName: "DateUpdated");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Dossiers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DossierId",
                table: "Documents",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Documents",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Dossiers_DossierId",
                table: "Documents",
                column: "DossierId",
                principalTable: "Dossiers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Dossiers_Id",
                table: "Documents",
                column: "Id",
                principalTable: "Dossiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
