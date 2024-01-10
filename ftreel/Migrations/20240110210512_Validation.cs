using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ftreel.Migrations
{
    /// <inheritdoc />
    public partial class Validation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValidated",
                table: "Documents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AMaKvs+/NKkjAlkRZwrMvQJrXEpSrCLzqb3PcOx4R29nQYYDp0ESm7MDjpLGOhYB1w==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "ANIC1X6veiRvSUAGmreuavpf8v0wAJ0soGVYGFFsr793zL79smmE0LoEC7alf2POPw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValidated",
                table: "Documents");

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
    }
}
