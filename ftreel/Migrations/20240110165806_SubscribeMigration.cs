using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ftreel.Migrations
{
    /// <inheritdoc />
    public partial class SubscribeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryUser",
                columns: table => new
                {
                    FollowedCategoriesId = table.Column<int>(type: "int", nullable: false),
                    FollowersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryUser", x => new { x.FollowedCategoriesId, x.FollowersId });
                    table.ForeignKey(
                        name: "FK_CategoryUser_Categories_FollowedCategoriesId",
                        column: x => x.FollowedCategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryUser_Users_FollowersId",
                        column: x => x.FollowersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CategoryUser_FollowersId",
                table: "CategoryUser",
                column: "FollowersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryUser");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AG5dNXoWPNS3eZnobVWcsIs+zh0oTH3frwDgzyhP7Vv2/EzlTbuGQdv/ENrWCN9pHQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "AHVQIo0OxFPwvqXmOmtix7otO4MqfVfp71O/kDJJI35YMaKuHVgtUmD8KrKMy/FUVw==");
        }
    }
}
