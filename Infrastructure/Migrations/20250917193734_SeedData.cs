using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FPIS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Concerts",
                columns: new[] { "Id", "AdditionalInfo", "Address", "City", "Name", "Venue" },
                values: new object[] { new Guid("d67a8082-12a4-4473-856d-b62ddcb6df88"), "Open air concert", "Piazza del Colosseo, 1", "Rome", "Eros Ramazzotti Live", "Colosseum" });

            migrationBuilder.InsertData(
                table: "Zones",
                columns: new[] { "Id", "Capacity", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("2c776d25-1891-4430-8117-514e61ecf8af"), 100, "VIP", 250.00m },
                    { new Guid("6ffb3268-48fe-49d3-8aed-84f9d8a1458d"), 500, "Regular", 100.00m }
                });

            migrationBuilder.InsertData(
                table: "ConcertDates",
                columns: new[] { "Id", "ConcertId", "Date" },
                values: new object[] { new Guid("f98191d0-2f69-4df5-b62b-290ff9c45ec2"), new Guid("d67a8082-12a4-4473-856d-b62ddcb6df88"), new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConcertDates",
                keyColumn: "Id",
                keyValue: new Guid("f98191d0-2f69-4df5-b62b-290ff9c45ec2"));

            migrationBuilder.DeleteData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: new Guid("2c776d25-1891-4430-8117-514e61ecf8af"));

            migrationBuilder.DeleteData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: new Guid("6ffb3268-48fe-49d3-8aed-84f9d8a1458d"));

            migrationBuilder.DeleteData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("d67a8082-12a4-4473-856d-b62ddcb6df88"));
        }
    }
}
