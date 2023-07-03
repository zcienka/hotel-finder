using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserIdToUserEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reservations",
                newName: "UserEmail");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "UserEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "Reservations",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "Comments",
                newName: "UserId");
        }
    }
}
