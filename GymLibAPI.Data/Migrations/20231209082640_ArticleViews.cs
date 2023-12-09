﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymLibAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class ArticleViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Articles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Views",
                table: "Articles");
        }
    }
}
