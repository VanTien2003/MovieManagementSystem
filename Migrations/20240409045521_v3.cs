using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banner",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banner", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BreakTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BusinessHours = table.Column<int>(type: "int", nullable: false),
                    CloseTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FixedTicketPrice = table.Column<double>(type: "float", nullable: false),
                    PercentDay = table.Column<int>(type: "int", nullable: false),
                    PercentWeekend = table.Column<int>(type: "int", nullable: false),
                    TimeBeginToChange = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralSetting", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banner");

            migrationBuilder.DropTable(
                name: "GeneralSetting");
        }
    }
}
