using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lunchbag.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "UserType" },
                values: new object[] { 1, "admin@lunchbag.de", "AQAAAAIAAYagAAAAECiJDuM8jVYjbYsiwbq2AU2xQL9pW28DS32O2xR1TFpEKpwASpkeN9NB5uu2KQvTxg", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
