using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tripex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ViewedCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewedCount",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewedCount",
                table: "Posts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewedCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ViewedCount",
                table: "Posts");
        }
    }
}
