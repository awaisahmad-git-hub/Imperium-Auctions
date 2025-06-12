using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImperiumAuctions.Migrations
{
    /// <inheritdoc />
    public partial class ChangesForStartingPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CountDownTargets_ProductID",
                table: "CountDownTargets");

            migrationBuilder.DropColumn(
                name: "DiscountPrice",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "StartingPrice");

            migrationBuilder.CreateIndex(
                name: "IX_CountDownTargets_ProductID",
                table: "CountDownTargets",
                column: "ProductID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CountDownTargets_ProductID",
                table: "CountDownTargets");

            migrationBuilder.RenameColumn(
                name: "StartingPrice",
                table: "Products",
                newName: "Price");

            migrationBuilder.AddColumn<double>(
                name: "DiscountPrice",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DiscountPrice",
                value: 80.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "DiscountPrice",
                value: 170.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "DiscountPrice",
                value: 130.0);

            migrationBuilder.CreateIndex(
                name: "IX_CountDownTargets_ProductID",
                table: "CountDownTargets",
                column: "ProductID");
        }
    }
}
