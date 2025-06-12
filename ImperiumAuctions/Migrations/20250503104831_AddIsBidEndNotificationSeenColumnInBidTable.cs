using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImperiumAuctions.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBidEndNotificationSeenColumnInBidTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBidEndNotificationSeen",
                table: "Bids",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBidEndNotificationSeen",
                table: "Bids");
        }
    }
}
