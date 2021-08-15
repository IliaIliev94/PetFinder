using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFinder.Data.Migrations
{
    public partial class ChangedSearchPostIdAndResourcePostIdFromNullableIntToStringForCommentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ResourcePosts_ResourcePostId1",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_SearchPosts_SearchPostId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ResourcePostId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_SearchPostId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ResourcePostId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "SearchPostId1",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "SearchPostId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ResourcePostId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ResourcePostId",
                table: "Comments",
                column: "ResourcePostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SearchPostId",
                table: "Comments",
                column: "SearchPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ResourcePosts_ResourcePostId",
                table: "Comments",
                column: "ResourcePostId",
                principalTable: "ResourcePosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_SearchPosts_SearchPostId",
                table: "Comments",
                column: "SearchPostId",
                principalTable: "SearchPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ResourcePosts_ResourcePostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_SearchPosts_SearchPostId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ResourcePostId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_SearchPostId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "SearchPostId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ResourcePostId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourcePostId1",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SearchPostId1",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ResourcePostId1",
                table: "Comments",
                column: "ResourcePostId1");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SearchPostId1",
                table: "Comments",
                column: "SearchPostId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ResourcePosts_ResourcePostId1",
                table: "Comments",
                column: "ResourcePostId1",
                principalTable: "ResourcePosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_SearchPosts_SearchPostId1",
                table: "Comments",
                column: "SearchPostId1",
                principalTable: "SearchPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
