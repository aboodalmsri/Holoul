using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holoul.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminReplyToFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReplyMessage",
                table: "FeedBacks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplyMessage",
                table: "FeedBacks");
        }
    }
}
