using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymLibAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class FoodFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Food_FoodEntityId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FoodEntityId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Bgu",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FoodEntityId",
                table: "Products");

            migrationBuilder.AddColumn<float>(
                name: "Carbohydrates",
                table: "Products",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Fats",
                table: "Products",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Proteins",
                table: "Products",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "FoodEntityProductEntity",
                columns: table => new
                {
                    FoodsId = table.Column<int>(type: "integer", nullable: false),
                    ProductsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodEntityProductEntity", x => new { x.FoodsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_FoodEntityProductEntity_Food_FoodsId",
                        column: x => x.FoodsId,
                        principalTable: "Food",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodEntityProductEntity_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodEntityProductEntity_ProductsId",
                table: "FoodEntityProductEntity",
                column: "ProductsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodEntityProductEntity");

            migrationBuilder.DropColumn(
                name: "Carbohydrates",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Fats",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Proteins",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Bgu",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FoodEntityId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FoodEntityId",
                table: "Products",
                column: "FoodEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Food_FoodEntityId",
                table: "Products",
                column: "FoodEntityId",
                principalTable: "Food",
                principalColumn: "Id");
        }
    }
}
