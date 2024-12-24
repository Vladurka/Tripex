using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tripex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PostsCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostsCount",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostsCount",
                table: "Users");
        }
    }
}
