using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ftreel.Migrations
{
    /// <inheritdoc />
    public partial class AuthorMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Documents");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AKRjO0sHxJ+hHoF+mtNh/B4LTNhEzgwkagvZM9YipudaUdj2eJXfBML3INTujC+MOQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "ANToyYKRLBkL9g32xC9RhyAu7wiw9J8ppZYSNG1p0lCjqudAEsPUpE5UUSSu9jW77w==");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AuthorId",
                table: "Documents",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_AuthorId",
                table: "Documents",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Users_AuthorId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_AuthorId",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Documents");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}
