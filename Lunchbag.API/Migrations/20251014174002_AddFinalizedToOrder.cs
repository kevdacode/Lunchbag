using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lunchbag.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFinalizedToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Finalized",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finalized",
                table: "Orders");
        }
    }
}
