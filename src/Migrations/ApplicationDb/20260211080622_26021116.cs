using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenClawWalletServer.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class _26021116 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SignType",
                table: "SignRecord",
                newName: "AddressType");

            migrationBuilder.RenameColumn(
                name: "SignType",
                table: "KeyConfig",
                newName: "AddressType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddressType",
                table: "SignRecord",
                newName: "SignType");

            migrationBuilder.RenameColumn(
                name: "AddressType",
                table: "KeyConfig",
                newName: "SignType");
        }
    }
}
