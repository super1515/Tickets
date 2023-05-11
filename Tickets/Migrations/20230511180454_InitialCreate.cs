using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Tickets.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "segments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: false),
                    patronymic = table.Column<string>(type: "text", nullable: false),
                    doc_type = table.Column<string>(type: "text", nullable: false),
                    doc_number = table.Column<string>(type: "text", nullable: false),
                    birthdate = table.Column<DateOnly>(type: "date", nullable: false),
                    gender = table.Column<string>(type: "text", nullable: false),
                    passenger_type = table.Column<string>(type: "text", nullable: false),
                    ticket_number = table.Column<string>(type: "text", nullable: false),
                    serial_number = table.Column<long>(type: "bigint", nullable: false),
                    ticket_type = table.Column<int>(type: "integer", nullable: false),
                    operation_type = table.Column<string>(type: "text", nullable: false),
                    operation_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    operation_place = table.Column<string>(type: "text", nullable: false),
                    airline_code = table.Column<string>(type: "text", nullable: false),
                    flight_num = table.Column<int>(type: "integer", nullable: false),
                    depart_place = table.Column<string>(type: "text", nullable: false),
                    depart_datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    arrive_place = table.Column<string>(type: "text", nullable: false),
                    arrive_datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pnr_id = table.Column<string>(type: "text", nullable: false),
                    operation_time_timezone = table.Column<string>(type: "text", nullable: false),
                    depart_datetime_timezone = table.Column<string>(type: "text", nullable: false),
                    arrive_datetime_timezone = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_segments", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_segments_ticket_number_serial_number",
                table: "segments",
                columns: new[] { "ticket_number", "serial_number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "segments");
        }
    }
}
