using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tripex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeWatchers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostWatchers_Posts_PostId",
                table: "PostWatchers");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "PostWatchers",
                newName: "EntityId");

            migrationBuilder.RenameIndex(
                name: "IX_PostWatchers_PostId",
                table: "PostWatchers",
                newName: "IX_PostWatchers_EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostWatchers_Posts_EntityId",
                table: "PostWatchers",
                column: "EntityId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostWatchers_Posts_EntityId",
                table: "PostWatchers");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "PostWatchers",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostWatchers_EntityId",
                table: "PostWatchers",
                newName: "IX_PostWatchers_PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostWatchers_Posts_PostId",
                table: "PostWatchers",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
