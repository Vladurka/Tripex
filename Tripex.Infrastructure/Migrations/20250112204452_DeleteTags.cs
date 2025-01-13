using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tripex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "Tags",
                table: "Posts",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }
    }
}
