using Microsoft.EntityFrameworkCore.Migrations;

namespace PetFinder.Data.Migrations
{
    public partial class MovedCityPropertyFromPetToSearchPostv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Cities_CityId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_CityId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Pets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Pets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_CityId",
                table: "Pets",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Cities_CityId",
                table: "Pets",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
