using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFinder.Data.Migrations
{
    public partial class FixedTypoInDescriptionProeprtyOfResourcePost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descripton",
                table: "ResourcePosts",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ResourcePosts",
                newName: "Descripton");
        }
    }
}
