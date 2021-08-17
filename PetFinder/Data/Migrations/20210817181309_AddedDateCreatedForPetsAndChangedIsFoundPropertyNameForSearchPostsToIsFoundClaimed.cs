using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFinder.Data.Migrations
{
    public partial class AddedDateCreatedForPetsAndChangedIsFoundPropertyNameForSearchPostsToIsFoundClaimed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFound",
                table: "SearchPosts",
                newName: "IsFoundClaimed");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Pets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Pets");

            migrationBuilder.RenameColumn(
                name: "IsFoundClaimed",
                table: "SearchPosts",
                newName: "IsFound");
        }
    }
}
