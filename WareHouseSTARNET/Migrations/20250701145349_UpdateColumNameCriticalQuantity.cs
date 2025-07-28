using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WareHouseSTARNET.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumNameCriticalQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CryticalQuantity",
                table: "Materials",
                newName: "CriticalQuantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CriticalQuantity",
                table: "Materials",
                newName: "CryticalQuantity");
        }
    }
}
