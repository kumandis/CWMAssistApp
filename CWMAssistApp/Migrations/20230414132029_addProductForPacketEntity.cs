using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CWMAssistApp.Migrations
{
    /// <inheritdoc />
    public partial class addProductForPacketEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Packets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "Packets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Packets");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "Packets");
        }
    }
}
