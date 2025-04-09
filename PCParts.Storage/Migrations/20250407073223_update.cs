using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCParts.Storage.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specification_Component_ComponentId",
                table: "Specification");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Specification");

            migrationBuilder.RenameColumn(
                name: "ComponentId",
                table: "Specification",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Specification_ComponentId",
                table: "Specification",
                newName: "IX_Specification_CategoryId");

            migrationBuilder.CreateTable(
                name: "SpecificationValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecificationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecificationValue_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecificationValue_Specification_SpecificationId",
                        column: x => x.SpecificationId,
                        principalTable: "Specification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationValue_ComponentId",
                table: "SpecificationValue",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationValue_SpecificationId",
                table: "SpecificationValue",
                column: "SpecificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Specification_Category_CategoryId",
                table: "Specification",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Specification_Category_CategoryId",
                table: "Specification");

            migrationBuilder.DropTable(
                name: "SpecificationValue");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Specification",
                newName: "ComponentId");

            migrationBuilder.RenameIndex(
                name: "IX_Specification_CategoryId",
                table: "Specification",
                newName: "IX_Specification_ComponentId");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Specification",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Specification_Component_ComponentId",
                table: "Specification",
                column: "ComponentId",
                principalTable: "Component",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
