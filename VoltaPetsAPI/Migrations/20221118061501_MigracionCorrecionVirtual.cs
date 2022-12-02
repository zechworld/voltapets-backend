using Microsoft.EntityFrameworkCore.Migrations;

namespace VoltaPetsAPI.Migrations
{
    public partial class MigracionCorrecionVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_paseador_experiencia_paseador_codigo_experiencia",
                table: "paseador");

            migrationBuilder.AlterColumn<int>(
                name: "codigo_experiencia",
                table: "paseador",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_paseador_experiencia_paseador_codigo_experiencia",
                table: "paseador",
                column: "codigo_experiencia",
                principalTable: "experiencia_paseador",
                principalColumn: "codigo_experiencia",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_paseador_experiencia_paseador_codigo_experiencia",
                table: "paseador");

            migrationBuilder.AlterColumn<int>(
                name: "codigo_experiencia",
                table: "paseador",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_paseador_experiencia_paseador_codigo_experiencia",
                table: "paseador",
                column: "codigo_experiencia",
                principalTable: "experiencia_paseador",
                principalColumn: "codigo_experiencia",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
