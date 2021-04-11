using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace br.com.mvc.lib.mngmt.repository.Migrations
{
    public partial class AddBorrowDateReturnDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Historics",
                newName: "ReturnDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "BorrowDate",
                table: "Historics",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowDate",
                table: "Historics");

            migrationBuilder.RenameColumn(
                name: "ReturnDate",
                table: "Historics",
                newName: "Date");
        }
    }
}
