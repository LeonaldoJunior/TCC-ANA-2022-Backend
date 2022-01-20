using Microsoft.EntityFrameworkCore.Migrations;

namespace TCC_Ana.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EndDevices",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndDeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DevEui = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DevAddr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GatewayId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GatewayEui = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceivedAt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FPort = table.Column<int>(type: "int", nullable: false),
                    FCnt = table.Column<int>(type: "int", nullable: false),
                    FrmPayload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnalogIn1 = table.Column<double>(type: "float", nullable: false),
                    AnalogIn2 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndDevices", x => x.EventId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndDevices");
        }
    }
}
