using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace VoltaPetsAPI.Migrations
{
    public partial class MigracionInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "calificacion",
                columns: table => new
                {
                    codigo_calificacion = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    valor = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_calificacion", x => x.codigo_calificacion);
                });

            migrationBuilder.CreateTable(
                name: "estado_mascota",
                columns: table => new
                {
                    codigo_estado_mascota = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estado_mascota", x => x.codigo_estado_mascota);
                });

            migrationBuilder.CreateTable(
                name: "estado_paseo",
                columns: table => new
                {
                    codigo_estado_paseo = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estado_paseo", x => x.codigo_estado_paseo);
                });

            migrationBuilder.CreateTable(
                name: "experiencia_paseador",
                columns: table => new
                {
                    codigo_experiencia = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_experiencia_paseador", x => x.codigo_experiencia);
                });

            migrationBuilder.CreateTable(
                name: "grupo_etario",
                columns: table => new
                {
                    codigo_etario = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(maxLength: 20, nullable: false),
                    edad_inferior = table.Column<string>(maxLength: 20, nullable: false),
                    edad_superior = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grupo_etario", x => x.codigo_etario);
                });

            migrationBuilder.CreateTable(
                name: "imagen",
                columns: table => new
                {
                    codigo_imagen = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    path = table.Column<string>(nullable: false),
                    url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_imagen", x => x.codigo_imagen);
                });

            migrationBuilder.CreateTable(
                name: "ponderacion",
                columns: table => new
                {
                    codigo_ponderacion = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    positivo = table.Column<float>(nullable: false),
                    negativo = table.Column<float>(nullable: false),
                    obediencia = table.Column<float>(nullable: false),
                    agresion = table.Column<float>(nullable: false),
                    fecha_registro = table.Column<DateTime>(nullable: false),
                    fecha_termino = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ponderacion", x => x.codigo_ponderacion);
                });

            migrationBuilder.CreateTable(
                name: "raza",
                columns: table => new
                {
                    codigo_raza = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(maxLength: 20, nullable: false),
                    ppp = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raza", x => x.codigo_raza);
                });

            migrationBuilder.CreateTable(
                name: "region",
                columns: table => new
                {
                    codigo_region = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_region", x => x.codigo_region);
                });

            migrationBuilder.CreateTable(
                name: "rol",
                columns: table => new
                {
                    codigo_rol = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol", x => x.codigo_rol);
                });

            migrationBuilder.CreateTable(
                name: "sexo",
                columns: table => new
                {
                    codigo_sexo = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sexo", x => x.codigo_sexo);
                });

            migrationBuilder.CreateTable(
                name: "tamanio",
                columns: table => new
                {
                    codigo_tamanio = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(maxLength: 20, nullable: false),
                    altura = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tamanio", x => x.codigo_tamanio);
                });

            migrationBuilder.CreateTable(
                name: "tipo_anuncio",
                columns: table => new
                {
                    codigo_tipo_anuncio = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_anuncio", x => x.codigo_tipo_anuncio);
                });

            migrationBuilder.CreateTable(
                name: "tipo_mascota",
                columns: table => new
                {
                    codigo_tipo_mascota = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_mascota", x => x.codigo_tipo_mascota);
                });

            migrationBuilder.CreateTable(
                name: "vacuna",
                columns: table => new
                {
                    codigo_vacuna = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(maxLength: 20, nullable: false),
                    periodo = table.Column<int>(nullable: false),
                    unidad_medida = table.Column<string>(maxLength: 20, nullable: false),
                    obligatoria = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vacuna", x => x.codigo_vacuna);
                });

            migrationBuilder.CreateTable(
                name: "perro_permitido",
                columns: table => new
                {
                    codigo_permitido = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tamanio_mediano = table.Column<bool>(nullable: false),
                    tamanio_grande = table.Column<bool>(nullable: false),
                    tamanio_gigante = table.Column<bool>(nullable: false),
                    codigo_experiencia = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perro_permitido", x => x.codigo_permitido);
                    table.ForeignKey(
                        name: "FK_perro_permitido_experiencia_paseador_codigo_experiencia",
                        column: x => x.codigo_experiencia,
                        principalTable: "experiencia_paseador",
                        principalColumn: "codigo_experiencia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "rango_tarifa",
                columns: table => new
                {
                    codigo_rango = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    basico_inferior = table.Column<int>(nullable: false),
                    basico_superior = table.Column<int>(nullable: false),
                    juego_inferior = table.Column<int>(nullable: false),
                    juego_superior = table.Column<int>(nullable: false),
                    social_inferior = table.Column<int>(nullable: false),
                    social_superior = table.Column<int>(nullable: false),
                    codigo_experiencia = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rango_tarifa", x => x.codigo_rango);
                    table.ForeignKey(
                        name: "FK_rango_tarifa_experiencia_paseador_codigo_experiencia",
                        column: x => x.codigo_experiencia,
                        principalTable: "experiencia_paseador",
                        principalColumn: "codigo_experiencia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "provincia",
                columns: table => new
                {
                    codigo_provincia = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(nullable: false),
                    codigo_region = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provincia", x => x.codigo_provincia);
                    table.ForeignKey(
                        name: "FK_provincia_region_codigo_region",
                        column: x => x.codigo_region,
                        principalTable: "region",
                        principalColumn: "codigo_region",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    codigo_usuario = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(maxLength: 200, nullable: false),
                    password = table.Column<string>(maxLength: 70, nullable: false),
                    token = table.Column<string>(nullable: false),
                    codigo_rol = table.Column<int>(nullable: false),
                    codigo_imagen = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.codigo_usuario);
                    table.ForeignKey(
                        name: "FK_usuario_imagen_codigo_imagen",
                        column: x => x.codigo_imagen,
                        principalTable: "imagen",
                        principalColumn: "codigo_imagen",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_usuario_rol_codigo_rol",
                        column: x => x.codigo_rol,
                        principalTable: "rol",
                        principalColumn: "codigo_rol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mascota_anuncio",
                columns: table => new
                {
                    codigo_mascota_anuncio = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(maxLength: 20, nullable: false),
                    edad = table.Column<int>(nullable: true),
                    codigo_tipo_mascota = table.Column<int>(nullable: false),
                    codigo_sexo = table.Column<int>(nullable: false),
                    codigo_tamanio = table.Column<int>(nullable: false),
                    codigo_raza = table.Column<int>(nullable: false),
                    codigo_etario = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mascota_anuncio", x => x.codigo_mascota_anuncio);
                    table.ForeignKey(
                        name: "FK_mascota_anuncio_grupo_etario_codigo_etario",
                        column: x => x.codigo_etario,
                        principalTable: "grupo_etario",
                        principalColumn: "codigo_etario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mascota_anuncio_raza_codigo_raza",
                        column: x => x.codigo_raza,
                        principalTable: "raza",
                        principalColumn: "codigo_raza",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mascota_anuncio_sexo_codigo_sexo",
                        column: x => x.codigo_sexo,
                        principalTable: "sexo",
                        principalColumn: "codigo_sexo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mascota_anuncio_tamanio_codigo_tamanio",
                        column: x => x.codigo_tamanio,
                        principalTable: "tamanio",
                        principalColumn: "codigo_tamanio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mascota_anuncio_tipo_mascota_codigo_tipo_mascota",
                        column: x => x.codigo_tipo_mascota,
                        principalTable: "tipo_mascota",
                        principalColumn: "codigo_tipo_mascota",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comuna",
                columns: table => new
                {
                    codigo_comuna = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(nullable: false),
                    codigo_provincia = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comuna", x => x.codigo_comuna);
                    table.ForeignKey(
                        name: "FK_comuna_provincia_codigo_provincia",
                        column: x => x.codigo_provincia,
                        principalTable: "provincia",
                        principalColumn: "codigo_provincia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "administrador",
                columns: table => new
                {
                    codigo_administrador = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(maxLength: 20, nullable: false),
                    apellido = table.Column<string>(maxLength: 20, nullable: false),
                    codigo_usuario = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrador", x => x.codigo_administrador);
                    table.ForeignKey(
                        name: "FK_administrador_usuario_codigo_usuario",
                        column: x => x.codigo_usuario,
                        principalTable: "usuario",
                        principalColumn: "codigo_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "parque_pet_friendly",
                columns: table => new
                {
                    codigo_parque = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(maxLength: 50, nullable: false),
                    latitud = table.Column<double>(nullable: false),
                    longitud = table.Column<double>(nullable: false),
                    codigo_comuna = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parque_pet_friendly", x => x.codigo_parque);
                    table.ForeignKey(
                        name: "FK_parque_pet_friendly_comuna_codigo_comuna",
                        column: x => x.codigo_comuna,
                        principalTable: "comuna",
                        principalColumn: "codigo_comuna",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ubicacion",
                columns: table => new
                {
                    codigo_ubicacion = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    direccion = table.Column<string>(maxLength: 200, nullable: false),
                    departamento = table.Column<int>(nullable: true),
                    latitud = table.Column<double>(nullable: false),
                    longitud = table.Column<double>(nullable: false),
                    codigo_comuna = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ubicacion", x => x.codigo_ubicacion);
                    table.ForeignKey(
                        name: "FK_ubicacion_comuna_codigo_comuna",
                        column: x => x.codigo_comuna,
                        principalTable: "comuna",
                        principalColumn: "codigo_comuna",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "paseador",
                columns: table => new
                {
                    codigo_paseador = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rut = table.Column<string>(maxLength: 11, nullable: false),
                    dv = table.Column<string>(maxLength: 1, nullable: false),
                    nombre = table.Column<string>(maxLength: 20, nullable: false),
                    apellido = table.Column<string>(maxLength: 20, nullable: false),
                    telefono = table.Column<string>(maxLength: 11, nullable: false),
                    descripcion = table.Column<string>(maxLength: 500, nullable: true),
                    activado = table.Column<bool>(nullable: false),
                    codigo_usuario = table.Column<int>(nullable: false),
                    codigo_ubicacion = table.Column<int>(nullable: false),
                    codigo_experiencia = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paseador", x => x.codigo_paseador);
                    table.ForeignKey(
                        name: "FK_paseador_experiencia_paseador_codigo_experiencia",
                        column: x => x.codigo_experiencia,
                        principalTable: "experiencia_paseador",
                        principalColumn: "codigo_experiencia",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_paseador_ubicacion_codigo_ubicacion",
                        column: x => x.codigo_ubicacion,
                        principalTable: "ubicacion",
                        principalColumn: "codigo_ubicacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_paseador_usuario_codigo_usuario",
                        column: x => x.codigo_usuario,
                        principalTable: "usuario",
                        principalColumn: "codigo_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tutor",
                columns: table => new
                {
                    codigo_tutor = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(maxLength: 20, nullable: false),
                    apellido = table.Column<string>(maxLength: 20, nullable: false),
                    telefono = table.Column<string>(maxLength: 11, nullable: false),
                    descripcion = table.Column<string>(maxLength: 500, nullable: true),
                    activado = table.Column<bool>(nullable: false),
                    codigo_usuario = table.Column<int>(nullable: false),
                    codigo_ubicacion = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tutor", x => x.codigo_tutor);
                    table.ForeignKey(
                        name: "FK_tutor_ubicacion_codigo_ubicacion",
                        column: x => x.codigo_ubicacion,
                        principalTable: "ubicacion",
                        principalColumn: "codigo_ubicacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tutor_usuario_codigo_usuario",
                        column: x => x.codigo_usuario,
                        principalTable: "usuario",
                        principalColumn: "codigo_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "paseo",
                columns: table => new
                {
                    codigo_paseo = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha = table.Column<DateTime>(nullable: false),
                    hora_inicio = table.Column<DateTime>(nullable: false),
                    hora_termino = table.Column<DateTime>(nullable: false),
                    duracion_basico = table.Column<TimeSpan>(nullable: false),
                    duracion_juego = table.Column<TimeSpan>(nullable: true),
                    duracion_social = table.Column<TimeSpan>(nullable: true),
                    calificado = table.Column<bool>(nullable: false),
                    codigo_paseador = table.Column<int>(nullable: false),
                    codigo_estado_paseo = table.Column<int>(nullable: false),
                    codigo_parque = table.Column<int>(nullable: true),
                    codigo_calificacion = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paseo", x => x.codigo_paseo);
                    table.ForeignKey(
                        name: "FK_paseo_calificacion_codigo_calificacion",
                        column: x => x.codigo_calificacion,
                        principalTable: "calificacion",
                        principalColumn: "codigo_calificacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_paseo_estado_paseo_codigo_estado_paseo",
                        column: x => x.codigo_estado_paseo,
                        principalTable: "estado_paseo",
                        principalColumn: "codigo_estado_paseo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_paseo_parque_pet_friendly_codigo_parque",
                        column: x => x.codigo_parque,
                        principalTable: "parque_pet_friendly",
                        principalColumn: "codigo_parque",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_paseo_paseador_codigo_paseador",
                        column: x => x.codigo_paseador,
                        principalTable: "paseador",
                        principalColumn: "codigo_paseador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "perro_aceptado",
                columns: table => new
                {
                    codigo_aceptado = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cantidad_perro = table.Column<int>(nullable: false),
                    tamanio_toy = table.Column<bool>(nullable: false),
                    tamanio_pequenio = table.Column<bool>(nullable: false),
                    tamanio_mediano = table.Column<bool>(nullable: false),
                    tamanio_grande = table.Column<bool>(nullable: false),
                    tamanio_gigante = table.Column<bool>(nullable: false),
                    codigo_paseador = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perro_aceptado", x => x.codigo_aceptado);
                    table.ForeignKey(
                        name: "FK_perro_aceptado_paseador_codigo_paseador",
                        column: x => x.codigo_paseador,
                        principalTable: "paseador",
                        principalColumn: "codigo_paseador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tarifa",
                columns: table => new
                {
                    codigo_tarifa = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    basico = table.Column<int>(nullable: false),
                    juego = table.Column<int>(nullable: false),
                    social = table.Column<int>(nullable: false),
                    fecha_registro = table.Column<DateTime>(nullable: false),
                    fecha_termino = table.Column<DateTime>(nullable: true),
                    codigo_paseador = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tarifa", x => x.codigo_tarifa);
                    table.ForeignKey(
                        name: "FK_tarifa_paseador_codigo_paseador",
                        column: x => x.codigo_paseador,
                        principalTable: "paseador",
                        principalColumn: "codigo_paseador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "anuncio",
                columns: table => new
                {
                    codigo_anuncio = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    titulo = table.Column<string>(maxLength: 100, nullable: true),
                    descripcion = table.Column<string>(maxLength: 500, nullable: false),
                    telefono = table.Column<string>(maxLength: 15, nullable: false),
                    recompensa = table.Column<int>(nullable: true),
                    responsable = table.Column<string>(maxLength: 50, nullable: false),
                    codigo_tutor = table.Column<int>(nullable: false),
                    codigo_tipo_anuncio = table.Column<int>(nullable: false),
                    codigo_mascota_anuncio = table.Column<int>(nullable: false),
                    codigo_imagen = table.Column<int>(nullable: false),
                    codigo_comuna = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_anuncio", x => x.codigo_anuncio);
                    table.ForeignKey(
                        name: "FK_anuncio_comuna_codigo_comuna",
                        column: x => x.codigo_comuna,
                        principalTable: "comuna",
                        principalColumn: "codigo_comuna",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_anuncio_imagen_codigo_imagen",
                        column: x => x.codigo_imagen,
                        principalTable: "imagen",
                        principalColumn: "codigo_imagen",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_anuncio_mascota_anuncio_codigo_mascota_anuncio",
                        column: x => x.codigo_mascota_anuncio,
                        principalTable: "mascota_anuncio",
                        principalColumn: "codigo_mascota_anuncio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_anuncio_tipo_anuncio_codigo_tipo_anuncio",
                        column: x => x.codigo_tipo_anuncio,
                        principalTable: "tipo_anuncio",
                        principalColumn: "codigo_tipo_anuncio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_anuncio_tutor_codigo_tutor",
                        column: x => x.codigo_tutor,
                        principalTable: "tutor",
                        principalColumn: "codigo_tutor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mascota",
                columns: table => new
                {
                    codigo_mascota = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(maxLength: 20, nullable: false),
                    descripcion = table.Column<string>(maxLength: 500, nullable: true),
                    fecha_nacimiento = table.Column<DateTime>(nullable: false),
                    esterilizado = table.Column<bool>(nullable: false),
                    edad_registro = table.Column<int>(nullable: false),
                    codigo_tutor = table.Column<int>(nullable: false),
                    codigo_sexo = table.Column<int>(nullable: false),
                    codigo_tamanio = table.Column<int>(nullable: false),
                    codigo_raza = table.Column<int>(nullable: false),
                    codigo_etario = table.Column<int>(nullable: false),
                    codigo_imagen = table.Column<int>(nullable: true),
                    codigo_estado_mascota = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mascota", x => x.codigo_mascota);
                    table.ForeignKey(
                        name: "FK_mascota_estado_mascota_codigo_estado_mascota",
                        column: x => x.codigo_estado_mascota,
                        principalTable: "estado_mascota",
                        principalColumn: "codigo_estado_mascota",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mascota_grupo_etario_codigo_etario",
                        column: x => x.codigo_etario,
                        principalTable: "grupo_etario",
                        principalColumn: "codigo_etario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mascota_imagen_codigo_imagen",
                        column: x => x.codigo_imagen,
                        principalTable: "imagen",
                        principalColumn: "codigo_imagen",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mascota_raza_codigo_raza",
                        column: x => x.codigo_raza,
                        principalTable: "raza",
                        principalColumn: "codigo_raza",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mascota_sexo_codigo_sexo",
                        column: x => x.codigo_sexo,
                        principalTable: "sexo",
                        principalColumn: "codigo_sexo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mascota_tamanio_codigo_tamanio",
                        column: x => x.codigo_tamanio,
                        principalTable: "tamanio",
                        principalColumn: "codigo_tamanio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mascota_tutor_codigo_tutor",
                        column: x => x.codigo_tutor,
                        principalTable: "tutor",
                        principalColumn: "codigo_tutor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "compromiso",
                columns: table => new
                {
                    codigo_compromiso = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    titulo = table.Column<string>(maxLength: 100, nullable: false),
                    fecha_compromiso = table.Column<DateTime>(nullable: false),
                    codigo_mascota = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compromiso", x => x.codigo_compromiso);
                    table.ForeignKey(
                        name: "FK_compromiso_mascota_codigo_mascota",
                        column: x => x.codigo_mascota,
                        principalTable: "mascota",
                        principalColumn: "codigo_mascota",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "paseo_mascota",
                columns: table => new
                {
                    codigo_paseo_mascota = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    evaluado = table.Column<bool>(nullable: false),
                    codigo_paseo = table.Column<int>(nullable: false),
                    codigo_mascota = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paseo_mascota", x => x.codigo_paseo_mascota);
                    table.ForeignKey(
                        name: "FK_paseo_mascota_mascota_codigo_mascota",
                        column: x => x.codigo_mascota,
                        principalTable: "mascota",
                        principalColumn: "codigo_mascota",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_paseo_mascota_paseo_codigo_paseo",
                        column: x => x.codigo_paseo,
                        principalTable: "paseo",
                        principalColumn: "codigo_paseo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recordatorio",
                columns: table => new
                {
                    codigo_recordatorio = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    titulo = table.Column<string>(maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(maxLength: 500, nullable: false),
                    fecha_publicacion = table.Column<DateTime>(nullable: false),
                    codigo_mascota = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recordatorio", x => x.codigo_recordatorio);
                    table.ForeignKey(
                        name: "FK_recordatorio_mascota_codigo_mascota",
                        column: x => x.codigo_mascota,
                        principalTable: "mascota",
                        principalColumn: "codigo_mascota",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vacuna_mascota",
                columns: table => new
                {
                    codigo_vacuna_mascota = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha_vacunacion = table.Column<DateTime>(nullable: false),
                    codigo_vacuna = table.Column<int>(nullable: false),
                    codigo_mascota = table.Column<int>(nullable: false),
                    codigo_imagen = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vacuna_mascota", x => x.codigo_vacuna_mascota);
                    table.ForeignKey(
                        name: "FK_vacuna_mascota_imagen_codigo_imagen",
                        column: x => x.codigo_imagen,
                        principalTable: "imagen",
                        principalColumn: "codigo_imagen",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vacuna_mascota_mascota_codigo_mascota",
                        column: x => x.codigo_mascota,
                        principalTable: "mascota",
                        principalColumn: "codigo_mascota",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vacuna_mascota_vacuna_codigo_vacuna",
                        column: x => x.codigo_vacuna,
                        principalTable: "vacuna",
                        principalColumn: "codigo_vacuna",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comportamiento",
                columns: table => new
                {
                    codigo_comportamiento = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nota_positivo = table.Column<float>(nullable: false),
                    nota_negativo = table.Column<float>(nullable: false),
                    nota_obediencia = table.Column<float>(nullable: false),
                    nota_agresion = table.Column<float>(nullable: false),
                    codigo_ponderacion = table.Column<int>(nullable: false),
                    codigo_paseo_mascota = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comportamiento", x => x.codigo_comportamiento);
                    table.ForeignKey(
                        name: "FK_comportamiento_paseo_mascota_codigo_paseo_mascota",
                        column: x => x.codigo_paseo_mascota,
                        principalTable: "paseo_mascota",
                        principalColumn: "codigo_paseo_mascota",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comportamiento_ponderacion_codigo_ponderacion",
                        column: x => x.codigo_ponderacion,
                        principalTable: "ponderacion",
                        principalColumn: "codigo_ponderacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "puntaje_personalidad",
                columns: table => new
                {
                    codigo_puntaje = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    amigable = table.Column<float>(nullable: false),
                    jugueton = table.Column<float>(nullable: false),
                    sociable = table.Column<float>(nullable: false),
                    nervioso = table.Column<float>(nullable: false),
                    timido = table.Column<float>(nullable: false),
                    miedoso = table.Column<float>(nullable: false),
                    dominante = table.Column<float>(nullable: false),
                    territorial = table.Column<float>(nullable: false),
                    agresivo = table.Column<float>(nullable: false),
                    codigo_paseo_mascota = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_puntaje_personalidad", x => x.codigo_puntaje);
                    table.ForeignKey(
                        name: "FK_puntaje_personalidad_paseo_mascota_codigo_paseo_mascota",
                        column: x => x.codigo_paseo_mascota,
                        principalTable: "paseo_mascota",
                        principalColumn: "codigo_paseo_mascota",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_administrador_codigo_usuario",
                table: "administrador",
                column: "codigo_usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_anuncio_codigo_comuna",
                table: "anuncio",
                column: "codigo_comuna");

            migrationBuilder.CreateIndex(
                name: "IX_anuncio_codigo_imagen",
                table: "anuncio",
                column: "codigo_imagen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_anuncio_codigo_mascota_anuncio",
                table: "anuncio",
                column: "codigo_mascota_anuncio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_anuncio_codigo_tipo_anuncio",
                table: "anuncio",
                column: "codigo_tipo_anuncio");

            migrationBuilder.CreateIndex(
                name: "IX_anuncio_codigo_tutor",
                table: "anuncio",
                column: "codigo_tutor");

            migrationBuilder.CreateIndex(
                name: "IX_comportamiento_codigo_paseo_mascota",
                table: "comportamiento",
                column: "codigo_paseo_mascota",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_comportamiento_codigo_ponderacion",
                table: "comportamiento",
                column: "codigo_ponderacion");

            migrationBuilder.CreateIndex(
                name: "IX_compromiso_codigo_mascota",
                table: "compromiso",
                column: "codigo_mascota");

            migrationBuilder.CreateIndex(
                name: "IX_comuna_codigo_provincia",
                table: "comuna",
                column: "codigo_provincia");

            migrationBuilder.CreateIndex(
                name: "IX_mascota_codigo_estado_mascota",
                table: "mascota",
                column: "codigo_estado_mascota");

            migrationBuilder.CreateIndex(
                name: "IX_mascota_codigo_etario",
                table: "mascota",
                column: "codigo_etario");

            migrationBuilder.CreateIndex(
                name: "IX_mascota_codigo_imagen",
                table: "mascota",
                column: "codigo_imagen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mascota_codigo_raza",
                table: "mascota",
                column: "codigo_raza");

            migrationBuilder.CreateIndex(
                name: "IX_mascota_codigo_sexo",
                table: "mascota",
                column: "codigo_sexo");

            migrationBuilder.CreateIndex(
                name: "IX_mascota_codigo_tamanio",
                table: "mascota",
                column: "codigo_tamanio");

            migrationBuilder.CreateIndex(
                name: "IX_mascota_codigo_tutor",
                table: "mascota",
                column: "codigo_tutor");

            migrationBuilder.CreateIndex(
                name: "IX_mascota_anuncio_codigo_etario",
                table: "mascota_anuncio",
                column: "codigo_etario");

            migrationBuilder.CreateIndex(
                name: "IX_mascota_anuncio_codigo_raza",
                table: "mascota_anuncio",
                column: "codigo_raza");

            migrationBuilder.CreateIndex(
                name: "IX_mascota_anuncio_codigo_sexo",
                table: "mascota_anuncio",
                column: "codigo_sexo");

            migrationBuilder.CreateIndex(
                name: "IX_mascota_anuncio_codigo_tamanio",
                table: "mascota_anuncio",
                column: "codigo_tamanio");

            migrationBuilder.CreateIndex(
                name: "IX_mascota_anuncio_codigo_tipo_mascota",
                table: "mascota_anuncio",
                column: "codigo_tipo_mascota");

            migrationBuilder.CreateIndex(
                name: "IX_parque_pet_friendly_codigo_comuna",
                table: "parque_pet_friendly",
                column: "codigo_comuna");

            migrationBuilder.CreateIndex(
                name: "IX_paseador_codigo_experiencia",
                table: "paseador",
                column: "codigo_experiencia");

            migrationBuilder.CreateIndex(
                name: "IX_paseador_codigo_ubicacion",
                table: "paseador",
                column: "codigo_ubicacion");

            migrationBuilder.CreateIndex(
                name: "IX_paseador_codigo_usuario",
                table: "paseador",
                column: "codigo_usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_paseo_codigo_calificacion",
                table: "paseo",
                column: "codigo_calificacion");

            migrationBuilder.CreateIndex(
                name: "IX_paseo_codigo_estado_paseo",
                table: "paseo",
                column: "codigo_estado_paseo");

            migrationBuilder.CreateIndex(
                name: "IX_paseo_codigo_parque",
                table: "paseo",
                column: "codigo_parque");

            migrationBuilder.CreateIndex(
                name: "IX_paseo_codigo_paseador",
                table: "paseo",
                column: "codigo_paseador");

            migrationBuilder.CreateIndex(
                name: "IX_paseo_mascota_codigo_mascota",
                table: "paseo_mascota",
                column: "codigo_mascota");

            migrationBuilder.CreateIndex(
                name: "IX_paseo_mascota_codigo_paseo",
                table: "paseo_mascota",
                column: "codigo_paseo");

            migrationBuilder.CreateIndex(
                name: "IX_perro_aceptado_codigo_paseador",
                table: "perro_aceptado",
                column: "codigo_paseador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_perro_permitido_codigo_experiencia",
                table: "perro_permitido",
                column: "codigo_experiencia",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_provincia_codigo_region",
                table: "provincia",
                column: "codigo_region");

            migrationBuilder.CreateIndex(
                name: "IX_puntaje_personalidad_codigo_paseo_mascota",
                table: "puntaje_personalidad",
                column: "codigo_paseo_mascota",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rango_tarifa_codigo_experiencia",
                table: "rango_tarifa",
                column: "codigo_experiencia",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_recordatorio_codigo_mascota",
                table: "recordatorio",
                column: "codigo_mascota");

            migrationBuilder.CreateIndex(
                name: "IX_tarifa_codigo_paseador",
                table: "tarifa",
                column: "codigo_paseador");

            migrationBuilder.CreateIndex(
                name: "IX_tutor_codigo_ubicacion",
                table: "tutor",
                column: "codigo_ubicacion");

            migrationBuilder.CreateIndex(
                name: "IX_tutor_codigo_usuario",
                table: "tutor",
                column: "codigo_usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ubicacion_codigo_comuna",
                table: "ubicacion",
                column: "codigo_comuna");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_codigo_imagen",
                table: "usuario",
                column: "codigo_imagen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_codigo_rol",
                table: "usuario",
                column: "codigo_rol");

            migrationBuilder.CreateIndex(
                name: "IX_vacuna_mascota_codigo_imagen",
                table: "vacuna_mascota",
                column: "codigo_imagen",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vacuna_mascota_codigo_mascota",
                table: "vacuna_mascota",
                column: "codigo_mascota");

            migrationBuilder.CreateIndex(
                name: "IX_vacuna_mascota_codigo_vacuna",
                table: "vacuna_mascota",
                column: "codigo_vacuna");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "administrador");

            migrationBuilder.DropTable(
                name: "anuncio");

            migrationBuilder.DropTable(
                name: "comportamiento");

            migrationBuilder.DropTable(
                name: "compromiso");

            migrationBuilder.DropTable(
                name: "perro_aceptado");

            migrationBuilder.DropTable(
                name: "perro_permitido");

            migrationBuilder.DropTable(
                name: "puntaje_personalidad");

            migrationBuilder.DropTable(
                name: "rango_tarifa");

            migrationBuilder.DropTable(
                name: "recordatorio");

            migrationBuilder.DropTable(
                name: "tarifa");

            migrationBuilder.DropTable(
                name: "vacuna_mascota");

            migrationBuilder.DropTable(
                name: "mascota_anuncio");

            migrationBuilder.DropTable(
                name: "tipo_anuncio");

            migrationBuilder.DropTable(
                name: "ponderacion");

            migrationBuilder.DropTable(
                name: "paseo_mascota");

            migrationBuilder.DropTable(
                name: "vacuna");

            migrationBuilder.DropTable(
                name: "tipo_mascota");

            migrationBuilder.DropTable(
                name: "mascota");

            migrationBuilder.DropTable(
                name: "paseo");

            migrationBuilder.DropTable(
                name: "estado_mascota");

            migrationBuilder.DropTable(
                name: "grupo_etario");

            migrationBuilder.DropTable(
                name: "raza");

            migrationBuilder.DropTable(
                name: "sexo");

            migrationBuilder.DropTable(
                name: "tamanio");

            migrationBuilder.DropTable(
                name: "tutor");

            migrationBuilder.DropTable(
                name: "calificacion");

            migrationBuilder.DropTable(
                name: "estado_paseo");

            migrationBuilder.DropTable(
                name: "parque_pet_friendly");

            migrationBuilder.DropTable(
                name: "paseador");

            migrationBuilder.DropTable(
                name: "experiencia_paseador");

            migrationBuilder.DropTable(
                name: "ubicacion");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "comuna");

            migrationBuilder.DropTable(
                name: "imagen");

            migrationBuilder.DropTable(
                name: "rol");

            migrationBuilder.DropTable(
                name: "provincia");

            migrationBuilder.DropTable(
                name: "region");
        }
    }
}
