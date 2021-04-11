using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace br.com.mvc.lib.mngmt.repository.Migrations
{
    public partial class addSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_UserId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_UserId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Books");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreateAt", "DeleteAt", "IsDeleted", "Name", "Password", "Roles", "UpdateAt", "Username" },
                values: new object[] { new Guid("311e33c8-b313-4087-9333-2bd0190f510e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, "Admin", "$2a$11$hbziCU9ZMLV7IDdh8vrQN.stbPfajyzB3H9hBiMM3wbTbPMp/p5Wa", "ADMIN", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admmin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("311e33c8-b313-4087-9333-2bd0190f510e"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Books",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserId",
                table: "Books",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_UserId",
                table: "Books",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
