using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CWMAssistApp.Migrations
{
    /// <inheritdoc />
    public partial class addPersonalWorkingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkingType",
                table: "Personals",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingType",
                table: "Personals");
        }
    }
}
