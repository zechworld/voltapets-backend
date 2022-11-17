using Microsoft.EntityFrameworkCore.Migrations;

namespace VoltaPetsAPI.Migrations
{
    public partial class MigracionImagenPublicID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "public_id",
                table: "imagen",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "public_id",
                table: "imagen");
        }
    }
}
