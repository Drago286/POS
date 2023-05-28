using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Taller1PIS.Migrations
{
    /// <inheritdoc />
    public partial class initialCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "POS");

            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "producto",
                schema: "POS",
                columns: table => new
                {
                    idproducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    codigo = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    descripcion = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    precio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idproducto);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "venta",
                schema: "POS",
                columns: table => new
                {
                    idventa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    correlativo = table.Column<double>(type: "double", nullable: false),
                    fecha = table.Column<DateTime>(type: "date", nullable: true),
                    hora = table.Column<TimeSpan>(type: "time", nullable: true),
                    rutCliente = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idventa);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "venta_producto",
                schema: "POS",
                columns: table => new
                {
                    idventa_producto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    idproducto = table.Column<int>(type: "int", nullable: true),
                    idventa = table.Column<int>(type: "int", nullable: true),
                    cantidad = table.Column<int>(type: "int", nullable: false),
                    precio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idventa_producto);
                    table.ForeignKey(
                        name: "idproducto",
                        column: x => x.idproducto,
                        principalSchema: "POS",
                        principalTable: "producto",
                        principalColumn: "idproducto");
                    table.ForeignKey(
                        name: "idventa",
                        column: x => x.idventa,
                        principalSchema: "POS",
                        principalTable: "venta",
                        principalColumn: "idventa");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "codigo_UNIQUE",
                schema: "POS",
                table: "producto",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "correlativo_UNIQUE",
                schema: "POS",
                table: "venta",
                column: "correlativo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idproducto_idx",
                schema: "POS",
                table: "venta_producto",
                column: "idproducto");

            migrationBuilder.CreateIndex(
                name: "idventa_idx",
                schema: "POS",
                table: "venta_producto",
                column: "idventa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "venta_producto",
                schema: "POS");

            migrationBuilder.DropTable(
                name: "producto",
                schema: "POS");

            migrationBuilder.DropTable(
                name: "venta",
                schema: "POS");
        }
    }
}
