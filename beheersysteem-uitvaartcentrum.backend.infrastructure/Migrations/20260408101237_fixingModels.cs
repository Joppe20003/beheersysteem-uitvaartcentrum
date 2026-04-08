using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace beheersysteem_uitvaartcentrum.backend.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixingModels : Migration
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

            migrationBuilder.AlterColumn<Guid>(
                name: "DossierId",
                table: "Documents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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
