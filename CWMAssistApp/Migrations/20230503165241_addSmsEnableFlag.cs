using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CWMAssistApp.Migrations
{
    /// <inheritdoc />
    public partial class addSmsEnableFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SendSmsEnable",
                table: "UserSmsPackets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendSmsEnable",
                table: "UserSmsPackets");
        }
    }
}
