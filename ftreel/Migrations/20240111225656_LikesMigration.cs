using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ftreel.Migrations
{
    /// <inheritdoc />
    public partial class LikesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentUser",
                columns: table => new
                {
                    LikedDocumentsId = table.Column<int>(type: "int", nullable: false),
                    likesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentUser", x => new { x.LikedDocumentsId, x.likesId });
                    table.ForeignKey(
                        name: "FK_DocumentUser_Documents_LikedDocumentsId",
                        column: x => x.LikedDocumentsId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentUser_Users_likesId",
                        column: x => x.likesId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "ANCFrki0f91MpTDt08v1E0/ElWZGhwYLkEn6lHl3v9bswUR1KEMQxFiZqL+1i2Y/Qg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "AL56Dj4LKgrLzd/OgBzz6LzBarwGUYo9xSLBwiM1QUg9VLfrVxr+9AEmWHoyDLayqA==");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentUser_likesId",
                table: "DocumentUser",
                column: "likesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentUser");

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
        }
    }
}
