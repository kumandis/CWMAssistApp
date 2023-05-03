using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CWMAssistApp.Migrations
{
    /// <inheritdoc />
    public partial class addCompanyIdForCustomerPackets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "CustomerPackets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CustomerPackets");
        }
    }
}
