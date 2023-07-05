using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Tickets.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Segments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Patronymic = table.Column<string>(type: "text", nullable: false),
                    DocType = table.Column<string>(type: "text", nullable: false),
                    DocNumber = table.Column<string>(type: "text", nullable: false),
                    Birthdate = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "text", nullable: false),
                    PassengerType = table.Column<string>(type: "text", nullable: false),
                    TicketNumber = table.Column<string>(type: "text", nullable: false),
                    SerialNumber = table.Column<long>(type: "bigint", nullable: false),
                    TicketType = table.Column<int>(type: "integer", nullable: false),
                    OperationType = table.Column<string>(type: "text", nullable: false),
                    OperationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OperationPlace = table.Column<string>(type: "text", nullable: false),
                    AirlineCode = table.Column<string>(type: "text", nullable: false),
                    FlightNum = table.Column<int>(type: "integer", nullable: false),
                    DepartPlace = table.Column<string>(type: "text", nullable: false),
                    DepartDatetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ArrivePlace = table.Column<string>(type: "text", nullable: false),
                    ArriveDatetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PnrId = table.Column<string>(type: "text", nullable: false),
                    OperationTimeTimezone = table.Column<string>(type: "text", nullable: false),
                    DepartDatetimeTimezone = table.Column<string>(type: "text", nullable: false),
                    ArriveDatetimeTimezone = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segments", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Segments_TicketNumber_SerialNumber",
                table: "Segments",
                columns: new[] { "TicketNumber", "SerialNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Segments");
        }
    }
}
