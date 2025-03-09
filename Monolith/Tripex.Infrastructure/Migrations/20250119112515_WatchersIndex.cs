using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tripex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WatchersIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_PostWatchers_UserId",
                table: "PostWatchers",
                newName: "IX_Watcher_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PostWatchers_EntityId",
                table: "PostWatchers",
                newName: "IX_Watcher_EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Watcher_UserId_EntityId",
                table: "PostWatchers",
                columns: new[] { "UserId", "EntityId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Watcher_UserId_EntityId",
                table: "PostWatchers");

            migrationBuilder.RenameIndex(
                name: "IX_Watcher_UserId",
                table: "PostWatchers",
                newName: "IX_PostWatchers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Watcher_EntityId",
                table: "PostWatchers",
                newName: "IX_PostWatchers_EntityId");
        }
    }
}
