namespace TechFix.Pro.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <summary>
    /// Initial database migration
    /// </summary>
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SerialNumber = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Manufacturer = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    DeviceName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    SoC = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Chipset = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Architecture = table.Column<int>(type: "INTEGER", nullable: false),
                    Platform = table.Column<int>(type: "INTEGER", nullable: false),
                    BootMode = table.Column<int>(type: "INTEGER", nullable: false),
                    ConnectionStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    UsbVendorId = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    UsbProductId = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    AndroidVersion = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    BuildNumber = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    FirmwareVersion = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    RAMGb = table.Column<int>(type: "INTEGER", nullable: true),
                    StorageGb = table.Column<int>(type: "INTEGER", nullable: true),
                    FirstDetectedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastDetectedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsAuthenticated = table.Column<bool>(type: "INTEGER", nullable: false),
                    IpAddress = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_SerialNumber",
                table: "Devices",
                column: "SerialNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Devices");
        }
    }
}