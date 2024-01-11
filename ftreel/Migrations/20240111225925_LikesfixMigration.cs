using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ftreel.Migrations
{
    /// <inheritdoc />
    public partial class LikesfixMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentUser_Users_likesId",
                table: "DocumentUser");

            migrationBuilder.RenameColumn(
                name: "likesId",
                table: "DocumentUser",
                newName: "LikesId");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentUser_likesId",
                table: "DocumentUser",
                newName: "IX_DocumentUser_LikesId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "AH+n0ze8s+5RH6NX7yDkGjaloTG/dH7E9au/TGh3FHDtFs1PxEbLuBoKDsl7XNiFlA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "AECgZchbnTYRA7OLtfittCaNVz0NhAB+Tx3m404BVXmmWPV752IQ9P/FxYeumKFqhQ==");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentUser_Users_LikesId",
                table: "DocumentUser",
                column: "LikesId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentUser_Users_LikesId",
                table: "DocumentUser");

            migrationBuilder.RenameColumn(
                name: "LikesId",
                table: "DocumentUser",
                newName: "likesId");

            migrationBuilder.RenameIndex(
                name: "IX_DocumentUser_LikesId",
                table: "DocumentUser",
                newName: "IX_DocumentUser_likesId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentUser_Users_likesId",
                table: "DocumentUser",
                column: "likesId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
