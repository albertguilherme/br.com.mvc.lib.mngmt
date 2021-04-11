using Microsoft.EntityFrameworkCore.Migrations;

namespace br.com.mvc.lib.mngmt.repository.Migrations
{
    public partial class RemovedTypeHistoric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Historics");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Historics",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
