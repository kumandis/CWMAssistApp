using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CWMAssistApp.Migrations
{
    /// <inheritdoc />
    public partial class addProductForAppointmentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Appointments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Appointments");
        }
    }
}
