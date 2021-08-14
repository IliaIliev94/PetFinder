using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFinder.Data.Migrations
{
    public partial class AddedImageUrlToResourcePost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ResourcePosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ResourcePosts");
        }
    }
}
