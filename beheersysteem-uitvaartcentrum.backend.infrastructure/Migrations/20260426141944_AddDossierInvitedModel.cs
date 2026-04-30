using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace beheersysteem_uitvaartcentrum.backend.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDossierInvitedModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitedUserIds",
                table: "Dossiers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<Guid>>(
                name: "InvitedUserIds",
                table: "Dossiers",
                type: "uuid[]",
                nullable: false);
        }
    }
}
