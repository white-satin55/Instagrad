using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Instagrad.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "Name",
            //    table: "Images",
            //    newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "MediaType",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Images",
                newName: "Name");
        }
    }
}
