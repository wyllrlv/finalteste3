using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace puc_projeto_eixo_2.Migrations
{
    /// <inheritdoc />
    public partial class descomentandoetestando : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Table_Treino",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avaliacao = table.Column<decimal>(type: "decimal(2,1)", precision: 2, scale: 1, nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table_Treino", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Table_Treino_Table_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Table_Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComentarioModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TreinoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComentarioModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComentarioModel_Table_Treino_TreinoId",
                        column: x => x.TreinoId,
                        principalTable: "Table_Treino",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExercicioModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TreinoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExercicioModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExercicioModel_Table_Treino_TreinoId",
                        column: x => x.TreinoId,
                        principalTable: "Table_Treino",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComentarioModel_TreinoId",
                table: "ComentarioModel",
                column: "TreinoId");

            migrationBuilder.CreateIndex(
                name: "IX_ExercicioModel_TreinoId",
                table: "ExercicioModel",
                column: "TreinoId");

            migrationBuilder.CreateIndex(
                name: "IX_Table_Treino_UsuarioId",
                table: "Table_Treino",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComentarioModel");

            migrationBuilder.DropTable(
                name: "ExercicioModel");

            migrationBuilder.DropTable(
                name: "Table_Treino");
        }
    }
}
