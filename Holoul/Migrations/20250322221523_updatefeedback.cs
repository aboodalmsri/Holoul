using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holoul.Migrations
{
    /// <inheritdoc />
    public partial class updatefeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FeedBacks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsReplied",
                table: "FeedBacks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RepliedAt",
                table: "FeedBacks",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FeedBacks");

            migrationBuilder.DropColumn(
                name: "IsReplied",
                table: "FeedBacks");

            migrationBuilder.DropColumn(
                name: "RepliedAt",
                table: "FeedBacks");
        }
    }
}
