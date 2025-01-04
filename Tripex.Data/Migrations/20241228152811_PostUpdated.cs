using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tripex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PostUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "Users",
                newName: "Updated");

            migrationBuilder.RenameColumn(
                name: "LastAvatarUpdated",
                table: "Users",
                newName: "AvatarUpdated");

            migrationBuilder.AddColumn<DateTime>(
                name: "ContentUrlUpdated",
                table: "Posts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentUrlUpdated",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Updated",
                table: "Users",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "AvatarUpdated",
                table: "Users",
                newName: "LastAvatarUpdated");
        }
    }
}
