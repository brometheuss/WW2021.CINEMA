using Microsoft.EntityFrameworkCore.Migrations;

namespace WinterWorkShop.Cinema.Data.Migrations
{
    public partial class addedcitytable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "city",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_city", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cinema_CityId",
                table: "cinema",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_cinema_city_CityId",
                table: "cinema",
                column: "CityId",
                principalTable: "city",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cinema_city_CityId",
                table: "cinema");

            migrationBuilder.DropTable(
                name: "city");

            migrationBuilder.DropIndex(
                name: "IX_cinema_CityId",
                table: "cinema");
        }
    }
}
