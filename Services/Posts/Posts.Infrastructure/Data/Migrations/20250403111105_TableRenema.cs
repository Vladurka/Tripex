using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Posts.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class TableRenema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PostsCounts",
                table: "PostsCounts");

            migrationBuilder.RenameTable(
                name: "PostsCounts",
                newName: "PostCount");

            migrationBuilder.RenameIndex(
                name: "IX_PostsCounts_ProfileId",
                table: "PostCount",
                newName: "IX_PostCount_ProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostCount",
                table: "PostCount",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PostCount",
                table: "PostCount");

            migrationBuilder.RenameTable(
                name: "PostCount",
                newName: "PostsCounts");

            migrationBuilder.RenameIndex(
                name: "IX_PostCount_ProfileId",
                table: "PostsCounts",
                newName: "IX_PostsCounts_ProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostsCounts",
                table: "PostsCounts",
                column: "Id");
        }
    }
}
