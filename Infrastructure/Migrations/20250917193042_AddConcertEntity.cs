using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPIS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConcertEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ConcertDateId",
                table: "ReservationTickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Concerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Concerts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConcertDates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConcertId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcertDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConcertDates_Concerts_ConcertId",
                        column: x => x.ConcertId,
                        principalTable: "Concerts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationTickets_ConcertDateId",
                table: "ReservationTickets",
                column: "ConcertDateId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcertDates_ConcertId",
                table: "ConcertDates",
                column: "ConcertId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationTickets_ConcertDates_ConcertDateId",
                table: "ReservationTickets",
                column: "ConcertDateId",
                principalTable: "ConcertDates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationTickets_ConcertDates_ConcertDateId",
                table: "ReservationTickets");

            migrationBuilder.DropTable(
                name: "ConcertDates");

            migrationBuilder.DropTable(
                name: "Concerts");

            migrationBuilder.DropIndex(
                name: "IX_ReservationTickets_ConcertDateId",
                table: "ReservationTickets");

            migrationBuilder.DropColumn(
                name: "ConcertDateId",
                table: "ReservationTickets");
        }
    }
}
