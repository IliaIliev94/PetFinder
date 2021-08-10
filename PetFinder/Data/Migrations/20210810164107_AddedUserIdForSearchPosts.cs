using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFinder.Data.Migrations
{
    public partial class AddedUserIdForSearchPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SearchPosts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SearchPosts_UserId",
                table: "SearchPosts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SearchPosts_AspNetUsers_UserId",
                table: "SearchPosts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SearchPosts_AspNetUsers_UserId",
                table: "SearchPosts");

            migrationBuilder.DropIndex(
                name: "IX_SearchPosts_UserId",
                table: "SearchPosts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SearchPosts");
        }
    }
}
