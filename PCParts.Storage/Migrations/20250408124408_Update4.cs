using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCParts.Storage.Migrations
{
    /// <inheritdoc />
    public partial class Update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SpecificationId",
                table: "SpecificationsValue",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationsValue_SpecificationId",
                table: "SpecificationsValue",
                column: "SpecificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificationsValue_Specification_SpecificationId",
                table: "SpecificationsValue",
                column: "SpecificationId",
                principalTable: "Specification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificationsValue_Specification_SpecificationId",
                table: "SpecificationsValue");

            migrationBuilder.DropIndex(
                name: "IX_SpecificationsValue_SpecificationId",
                table: "SpecificationsValue");

            migrationBuilder.DropColumn(
                name: "SpecificationId",
                table: "SpecificationsValue");
        }
    }
}
