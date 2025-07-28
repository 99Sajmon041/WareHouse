using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHouseSTARNET.Migrations
{
    /// <inheritdoc />
    public partial class SetCascadeDeletingOnModelCreatingBuilder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WrittenOffMaterials_AspNetUsers_ApplicationUserId",
                table: "WrittenOffMaterials");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "WrittenOffMaterials",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_WrittenOffMaterials_AspNetUsers_ApplicationUserId",
                table: "WrittenOffMaterials",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WrittenOffMaterials_AspNetUsers_ApplicationUserId",
                table: "WrittenOffMaterials");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "WrittenOffMaterials",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WrittenOffMaterials_AspNetUsers_ApplicationUserId",
                table: "WrittenOffMaterials",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
