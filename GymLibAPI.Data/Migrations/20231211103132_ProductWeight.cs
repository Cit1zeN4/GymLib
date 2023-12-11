using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GymLibAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductWeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodEntityProductEntity");

            migrationBuilder.AddColumn<int>(
                name: "ProductEntityId",
                table: "Food",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductWeight",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    FoodEntityId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductWeight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductWeight_Food_FoodEntityId",
                        column: x => x.FoodEntityId,
                        principalTable: "Food",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductWeight_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Food_ProductEntityId",
                table: "Food",
                column: "ProductEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWeight_FoodEntityId",
                table: "ProductWeight",
                column: "FoodEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWeight_ProductId",
                table: "ProductWeight",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Food_Products_ProductEntityId",
                table: "Food",
                column: "ProductEntityId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Food_Products_ProductEntityId",
                table: "Food");

            migrationBuilder.DropTable(
                name: "ProductWeight");

            migrationBuilder.DropIndex(
                name: "IX_Food_ProductEntityId",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "ProductEntityId",
                table: "Food");

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
    }
}
