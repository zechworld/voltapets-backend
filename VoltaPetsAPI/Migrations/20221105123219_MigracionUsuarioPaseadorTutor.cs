using Microsoft.EntityFrameworkCore.Migrations;

namespace VoltaPetsAPI.Migrations
{
    public partial class MigracionUsuarioPaseadorTutor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_paseador_experiencia_paseador_codigo_experiencia",
                table: "paseador");

            migrationBuilder.AlterColumn<string>(
                name: "telefono",
                table: "tutor",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "tutor",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "apellido",
                table: "tutor",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "telefono",
                table: "paseador",
                maxLength: 12,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "paseador",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "codigo_experiencia",
                table: "paseador",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "apellido",
                table: "paseador",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddForeignKey(
                name: "FK_paseador_experiencia_paseador_codigo_experiencia",
                table: "paseador",
                column: "codigo_experiencia",
                principalTable: "experiencia_paseador",
                principalColumn: "codigo_experiencia",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_paseador_experiencia_paseador_codigo_experiencia",
                table: "paseador");

            migrationBuilder.AlterColumn<string>(
                name: "telefono",
                table: "tutor",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "tutor",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<string>(
                name: "apellido",
                table: "tutor",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<string>(
                name: "telefono",
                table: "paseador",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "nombre",
                table: "paseador",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<int>(
                name: "codigo_experiencia",
                table: "paseador",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "apellido",
                table: "paseador",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 40);

            migrationBuilder.AddForeignKey(
                name: "FK_paseador_experiencia_paseador_codigo_experiencia",
                table: "paseador",
                column: "codigo_experiencia",
                principalTable: "experiencia_paseador",
                principalColumn: "codigo_experiencia",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
