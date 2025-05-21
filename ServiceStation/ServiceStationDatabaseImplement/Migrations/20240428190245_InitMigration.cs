using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceStationDatabaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Executors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExecutorFIO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutorEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExecutorPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutorNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Executors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Guarantors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuarantorFIO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuarantorEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuarantorPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuarantorNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guarantors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarBrand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExecutorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_Executors_ExecutorId",
                        column: x => x.ExecutorId,
                        principalTable: "Executors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalWorks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateStartWork = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WorkPrice = table.Column<double>(type: "float", nullable: false),
                    ExecutorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalWorks_Executors_ExecutorId",
                        column: x => x.ExecutorId,
                        principalTable: "Executors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Repairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RepairPrice = table.Column<double>(type: "float", nullable: false),
                    GuarantorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repairs_Guarantors_GuarantorId",
                        column: x => x.GuarantorId,
                        principalTable: "Guarantors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpareParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SparePartName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SparePartPrice = table.Column<double>(type: "float", nullable: false),
                    GuarantorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpareParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpareParts_Guarantors_GuarantorId",
                        column: x => x.GuarantorId,
                        principalTable: "Guarantors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarTechnicalWorks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    TechnicalWorkId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarTechnicalWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarTechnicalWorks_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarTechnicalWorks_TechnicalWorks_TechnicalWorkId",
                        column: x => x.TechnicalWorkId,
                        principalTable: "TechnicalWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    WorkPrice = table.Column<double>(type: "float", nullable: false),
                    GuarantorId = table.Column<int>(type: "int", nullable: false),
                    TechnicalWorkId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Works_Guarantors_GuarantorId",
                        column: x => x.GuarantorId,
                        principalTable: "Guarantors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Works_TechnicalWorks_TechnicalWorkId",
                        column: x => x.TechnicalWorkId,
                        principalTable: "TechnicalWorks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Defects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefectType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefectPrice = table.Column<double>(type: "float", nullable: false),
                    ExecutorId = table.Column<int>(type: "int", nullable: false),
                    RepairId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Defects_Executors_ExecutorId",
                        column: x => x.ExecutorId,
                        principalTable: "Executors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Defects_Repairs_RepairId",
                        column: x => x.RepairId,
                        principalTable: "Repairs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SparePartRepairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SparePartId = table.Column<int>(type: "int", nullable: false),
                    RepairId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SparePartRepairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SparePartRepairs_SpareParts_SparePartId",
                        column: x => x.SparePartId,
                        principalTable: "SpareParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SparePartRepairs_Repairs_RepairId",
                        column: x => x.RepairId,
                        principalTable: "Repairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SparePartWorks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SparePartId = table.Column<int>(type: "int", nullable: false),
                    WorkId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SparePartWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SparePartWorks_SpareParts_SparePartId",
                        column: x => x.SparePartId,
                        principalTable: "SpareParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SparePartWorks_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CarDefects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    DefectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarDefects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarDefects_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarDefects_Defects_DefectId",
                        column: x => x.DefectId,
                        principalTable: "Defects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarDefects_CarId",
                table: "CarDefects",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarDefects_DefectId",
                table: "CarDefects",
                column: "DefectId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_ExecutorId",
                table: "Cars",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_CarTechnicalWorks_CarId",
                table: "CarTechnicalWorks",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarTechnicalWorks_TechnicalWorkId",
                table: "CarTechnicalWorks",
                column: "TechnicalWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_ExecutorId",
                table: "Defects",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_RepairId",
                table: "Defects",
                column: "RepairId");

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_GuarantorId",
                table: "Repairs",
                column: "GuarantorId");

            migrationBuilder.CreateIndex(
                name: "IX_SparePartRepairs_SparePartId",
                table: "SparePartRepairs",
                column: "SparePartId");

            migrationBuilder.CreateIndex(
                name: "IX_SparePartRepairs_RepairId",
                table: "SparePartRepairs",
                column: "RepairId");

            migrationBuilder.CreateIndex(
                name: "IX_SpareParts_GuarantorId",
                table: "SpareParts",
                column: "GuarantorId");

            migrationBuilder.CreateIndex(
                name: "IX_SparePartWorks_SparePartId",
                table: "SparePartWorks",
                column: "SparePartId");

            migrationBuilder.CreateIndex(
                name: "IX_SparePartWorks_WorkId",
                table: "SparePartWorks",
                column: "WorkId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalWorks_ExecutorId",
                table: "TechnicalWorks",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_GuarantorId",
                table: "Works",
                column: "GuarantorId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_TechnicalWorkId",
                table: "Works",
                column: "TechnicalWorkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarDefects");

            migrationBuilder.DropTable(
                name: "CarTechnicalWorks");

            migrationBuilder.DropTable(
                name: "SparePartRepairs");

            migrationBuilder.DropTable(
                name: "SparePartWorks");

            migrationBuilder.DropTable(
                name: "Defects");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "SpareParts");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "Repairs");

            migrationBuilder.DropTable(
                name: "TechnicalWorks");

            migrationBuilder.DropTable(
                name: "Guarantors");

            migrationBuilder.DropTable(
                name: "Executors");
        }
    }
}
