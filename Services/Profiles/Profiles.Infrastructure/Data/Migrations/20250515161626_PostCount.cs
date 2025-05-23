using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profiles.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class PostCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FollowersCount",
                table: "Profiles",
                newName: "PostCount");

            migrationBuilder.AddColumn<int>(
                name: "FollowerCount",
                table: "Profiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowerCount",
                table: "Profiles");

            migrationBuilder.RenameColumn(
                name: "PostCount",
                table: "Profiles",
                newName: "FollowersCount");
        }
    }
}
