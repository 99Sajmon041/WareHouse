using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHouseSTARNET.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTypeOfMaterialIdFromWrittenOffMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WrittenOffMaterials_TypeOfmaterials_TypeOfMaterialId",
                table: "WrittenOffMaterials");

            migrationBuilder.DropIndex(
                name: "IX_WrittenOffMaterials_TypeOfMaterialId",
                table: "WrittenOffMaterials");

            migrationBuilder.DropColumn(
                name: "TypeOfMaterialId",
                table: "WrittenOffMaterials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeOfMaterialId",
                table: "WrittenOffMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WrittenOffMaterials_TypeOfMaterialId",
                table: "WrittenOffMaterials",
                column: "TypeOfMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_WrittenOffMaterials_TypeOfmaterials_TypeOfMaterialId",
                table: "WrittenOffMaterials",
                column: "TypeOfMaterialId",
                principalTable: "TypeOfmaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
