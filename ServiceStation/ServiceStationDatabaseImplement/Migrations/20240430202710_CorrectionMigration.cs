using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceStationDatabaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class CorrectionMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Repairs");

            migrationBuilder.AddColumn<DateTime>(
                name: "RepairStartDate",
                table: "Repairs",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepairStartDate",
                table: "Repairs");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Repairs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
