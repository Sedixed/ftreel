using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ftreel.Migrations
{
    /// <inheritdoc />
    public partial class UserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Mail", "Password", "Roles" },
                values: new object[,]
                {
                    { 1, "admin@ftreel.com", "AG5dNXoWPNS3eZnobVWcsIs+zh0oTH3frwDgzyhP7Vv2/EzlTbuGQdv/ENrWCN9pHQ==", "ROLE_ADMIN" },
                    { 2, "user@ftreel.com", "AHVQIo0OxFPwvqXmOmtix7otO4MqfVfp71O/kDJJI35YMaKuHVgtUmD8KrKMy/FUVw==", "ROLE_USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
