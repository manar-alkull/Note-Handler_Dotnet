using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoteManager.Migrations
{
    /// <inheritdoc />
    public partial class migration5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachedImage",
                table: "Notes");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Notes");

            migrationBuilder.AddColumn<byte[]>(
                name: "AttachedImage",
                table: "Notes",
                type: "image",
                nullable: true);
        }
    }
}
