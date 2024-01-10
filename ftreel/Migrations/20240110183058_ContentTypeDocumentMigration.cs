using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ftreel.Migrations
{
    /// <inheritdoc />
    public partial class ContentTypeDocumentMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Extension",
                table: "Documents",
                newName: "ContentType");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AKUEwjg5wY64493iO1feEHqwdUwhKshENBsm7KCjAo6FiPTp6pa6NAyXQqzqRT4x1w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "ANyVPLXq2ZDVRlTgkN5WBIUGLEBn5BaET5ZN3YJeZVdkXDOw3L9dCtE/rY3g41eWDw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "Documents",
                newName: "Extension");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AJ1RmB7G5oRsk+ZXhLxENOKMpF1NuvruZC5IJlm0qa6XKozKtnJoCpcw0xHrlovaiw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "AHu0ZPtR7KRDsB2jENnDfw3m58r366yvEumR+UFOAwIcrECMXEBEaGdq2ebX2B+YQw==");
        }
    }
}
