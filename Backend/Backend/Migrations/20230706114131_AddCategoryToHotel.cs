using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToHotel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomsNumber",
                table: "Hotels");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Hotels",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Hotels");

            migrationBuilder.AddColumn<int>(
                name: "RoomsNumber",
                table: "Hotels",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
