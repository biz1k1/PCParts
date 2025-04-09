using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCParts.Storage.Migrations
{
    /// <inheritdoc />
    public partial class Update6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificationsValue_Component_ComponentId",
                table: "SpecificationsValue");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecificationsValue_Specification_SpecificationId",
                table: "SpecificationsValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecificationsValue",
                table: "SpecificationsValue");

            migrationBuilder.RenameTable(
                name: "SpecificationsValue",
                newName: "SpecificationValue");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificationsValue_SpecificationId",
                table: "SpecificationValue",
                newName: "IX_SpecificationValue_SpecificationId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificationsValue_ComponentId",
                table: "SpecificationValue",
                newName: "IX_SpecificationValue_ComponentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecificationValue",
                table: "SpecificationValue",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificationValue_Component_ComponentId",
                table: "SpecificationValue",
                column: "ComponentId",
                principalTable: "Component",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificationValue_Specification_SpecificationId",
                table: "SpecificationValue",
                column: "SpecificationId",
                principalTable: "Specification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificationValue_Component_ComponentId",
                table: "SpecificationValue");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecificationValue_Specification_SpecificationId",
                table: "SpecificationValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecificationValue",
                table: "SpecificationValue");

            migrationBuilder.RenameTable(
                name: "SpecificationValue",
                newName: "SpecificationsValue");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificationValue_SpecificationId",
                table: "SpecificationsValue",
                newName: "IX_SpecificationsValue_SpecificationId");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificationValue_ComponentId",
                table: "SpecificationsValue",
                newName: "IX_SpecificationsValue_ComponentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecificationsValue",
                table: "SpecificationsValue",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificationsValue_Component_ComponentId",
                table: "SpecificationsValue",
                column: "ComponentId",
                principalTable: "Component",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificationsValue_Specification_SpecificationId",
                table: "SpecificationsValue",
                column: "SpecificationId",
                principalTable: "Specification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
