using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profiles.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ViewCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Profiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Id",
                table: "Profiles",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_ProfileName",
                table: "Profiles",
                column: "ProfileName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_Id",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_ProfileName",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Profiles");
        }
    }
}
