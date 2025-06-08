using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCParts.Storage.Migrations
{
    /// <inheritdoc />
    public partial class update7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attempts",
                table: "PendingUsers");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "PendingUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PendingUsers");

            migrationBuilder.AddColumn<int>(
                name: "Attempts",
                table: "PendingUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
