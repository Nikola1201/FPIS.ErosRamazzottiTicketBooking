using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FPIS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

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
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConfirmedEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "AccessTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    ReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromoCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LinkedReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsedPromoCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GeneratedPromoCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_PromoCodes_UsedPromoCodeId",
                        column: x => x.UsedPromoCodeId,
                        principalTable: "PromoCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ReservationTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ZoneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ConcertDateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationTickets_ConcertDates_ConcertDateId",
                        column: x => x.ConcertDateId,
                        principalTable: "ConcertDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationTickets_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationTickets_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AppSettings",
                columns: new[] { "Id", "Key", "Value" },
                values: new object[,]
                {
                    { new Guid("1026b45e-1aa1-4d6a-9e72-40f9b58871f6"), "EarlyBirdDiscountPercentage", "10" },
                    { new Guid("1fbec0d0-8fd4-4ce8-b62d-2533ff55ff06"), "EarlyBirdDiscountDaysBefore", "60" },
                    { new Guid("5eb344ef-9e7a-4a78-8e3e-23b11f1ed65a"), "FriendPromoDiscountPercentage", "5" },
                    { new Guid("ddc2be4a-bad6-4602-b34f-2a7b9b5f1502"), "FifthTicketDiscountPercentage", "50" }
                });

            migrationBuilder.InsertData(
                table: "Concerts",
                columns: new[] { "Id", "AdditionalInfo", "Address", "City", "Name", "Venue" },
                values: new object[] { new Guid("a0ef757c-4fae-46b4-8b20-a0ebc4b9eb7e"), "Open air concert", "Piazza del Colosseo, 1", "Rome", "Eros Ramazzotti Live", "Colosseum" });

            migrationBuilder.InsertData(
                table: "Zones",
                columns: new[] { "Id", "Capacity", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("111d640b-63d2-42bc-b5a6-79f77463d4fd"), 100, "VIP", 250.00m },
                    { new Guid("4e9c45c0-a41a-473a-88ad-ef4276bb5301"), 500, "Regular", 100.00m }
                });

            migrationBuilder.InsertData(
                table: "ConcertDates",
                columns: new[] { "Id", "ConcertId", "Date" },
                values: new object[,]
                {
                    { new Guid("7af48f46-bb1c-4efa-bfe9-37c57de10c00"), new Guid("a0ef757c-4fae-46b4-8b20-a0ebc4b9eb7e"), new DateTime(2025, 11, 19, 21, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a9f9d5ac-9fe2-4141-a1a1-e7e2dba34e4f"), new Guid("a0ef757c-4fae-46b4-8b20-a0ebc4b9eb7e"), new DateTime(2026, 11, 17, 21, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("faccd7a9-7bab-47a8-9300-296b34a8809e"), new Guid("a0ef757c-4fae-46b4-8b20-a0ebc4b9eb7e"), new DateTime(2027, 11, 15, 21, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessTokens_ReservationId",
                table: "AccessTokens",
                column: "ReservationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppSettings_Key",
                table: "AppSettings",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConcertDates_ConcertId",
                table: "ConcertDates",
                column: "ConcertId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_ReservationId",
                table: "Discounts",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_LinkedReservationId",
                table: "PromoCodes",
                column: "LinkedReservationId",
                unique: true,
                filter: "[LinkedReservationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CustomerId",
                table: "Reservations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UsedPromoCodeId",
                table: "Reservations",
                column: "UsedPromoCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationTickets_ConcertDateId",
                table: "ReservationTickets",
                column: "ConcertDateId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationTickets_ReservationId",
                table: "ReservationTickets",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationTickets_ZoneId",
                table: "ReservationTickets",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessTokens_Reservations_ReservationId",
                table: "AccessTokens",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Reservations_ReservationId",
                table: "Discounts",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Reservations_LinkedReservationId",
                table: "PromoCodes",
                column: "LinkedReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Reservations_LinkedReservationId",
                table: "PromoCodes");

            migrationBuilder.DropTable(
                name: "AccessTokens");

            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "ReservationTickets");

            migrationBuilder.DropTable(
                name: "ConcertDates");

            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropTable(
                name: "Concerts");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "PromoCodes");
        }
    }
}
