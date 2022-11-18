using Microsoft.EntityFrameworkCore.Migrations;

namespace VoltaPetsAPI.Migrations
{
    public partial class MigracionNotNullFkImagenMascota : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mascota_imagen_codigo_imagen",
                table: "mascota");

            migrationBuilder.AlterColumn<int>(
                name: "codigo_imagen",
                table: "mascota",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_mascota_imagen_codigo_imagen",
                table: "mascota",
                column: "codigo_imagen",
                principalTable: "imagen",
                principalColumn: "codigo_imagen",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mascota_imagen_codigo_imagen",
                table: "mascota");

            migrationBuilder.AlterColumn<int>(
                name: "codigo_imagen",
                table: "mascota",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_mascota_imagen_codigo_imagen",
                table: "mascota",
                column: "codigo_imagen",
                principalTable: "imagen",
                principalColumn: "codigo_imagen",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
