using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCParts.Storage.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePendingUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmsCode",
                table: "PendingUsers");

            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationToken",
                table: "PendingUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmed",
                table: "PendingUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmationToken",
                table: "PendingUsers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "PendingUsers");

            migrationBuilder.AddColumn<string>(
                name: "SmsCode",
                table: "PendingUsers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
