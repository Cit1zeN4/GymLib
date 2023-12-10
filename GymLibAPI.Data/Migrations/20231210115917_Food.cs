using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GymLibAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class Food : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FoodEntityId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Food",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Food_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_FoodEntityId",
                table: "Products",
                column: "FoodEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Food_UserId",
                table: "Food",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Food_FoodEntityId",
                table: "Products",
                column: "FoodEntityId",
                principalTable: "Food",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Food_FoodEntityId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Food");

            migrationBuilder.DropIndex(
                name: "IX_Products_FoodEntityId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FoodEntityId",
                table: "Products");
        }
    }
}
