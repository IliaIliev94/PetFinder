using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFinder.Models.Migrations
{
    public partial class ChangedPetIdFromIntToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SearchPosts_Pets_PetId1",
                table: "SearchPosts");

            migrationBuilder.DropIndex(
                name: "IX_SearchPosts_PetId1",
                table: "SearchPosts");

            migrationBuilder.DropColumn(
                name: "PetId1",
                table: "SearchPosts");

            migrationBuilder.AlterColumn<string>(
                name: "PetId",
                table: "SearchPosts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_SearchPosts_PetId",
                table: "SearchPosts",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_SearchPosts_Pets_PetId",
                table: "SearchPosts",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SearchPosts_Pets_PetId",
                table: "SearchPosts");

            migrationBuilder.DropIndex(
                name: "IX_SearchPosts_PetId",
                table: "SearchPosts");

            migrationBuilder.AlterColumn<int>(
                name: "PetId",
                table: "SearchPosts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PetId1",
                table: "SearchPosts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SearchPosts_PetId1",
                table: "SearchPosts",
                column: "PetId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SearchPosts_Pets_PetId1",
                table: "SearchPosts",
                column: "PetId1",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
