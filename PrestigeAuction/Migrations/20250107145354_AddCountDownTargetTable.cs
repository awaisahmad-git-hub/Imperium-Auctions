using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrestigeAuction.Migrations
{
    /// <inheritdoc />
    public partial class AddCountDownTargetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountDownTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTargetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTargetDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountDownTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountDownTargets_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountDownTargets_ProductID",
                table: "CountDownTargets",
                column: "ProductID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountDownTargets");
        }
    }
}
