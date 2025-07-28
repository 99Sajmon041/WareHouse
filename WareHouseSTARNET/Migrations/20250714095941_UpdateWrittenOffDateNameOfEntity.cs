using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHouseSTARNET.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWrittenOffDateNameOfEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WritteOfDate",
                table: "WrittenOffMaterials",
                newName: "WrittenOffDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WrittenOffDate",
                table: "WrittenOffMaterials",
                newName: "WritteOfDate");
        }
    }
}
