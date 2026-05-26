using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FPIS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PromoCodeFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Reservations_LinkedReservationId",
                table: "PromoCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_PromoCodes_UsedPromoCodeId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_UsedPromoCodeId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_PromoCodes_LinkedReservationId",
                table: "PromoCodes");

            migrationBuilder.DeleteData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: new Guid("1026b45e-1aa1-4d6a-9e72-40f9b58871f6"));

            migrationBuilder.DeleteData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: new Guid("1fbec0d0-8fd4-4ce8-b62d-2533ff55ff06"));

            migrationBuilder.DeleteData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: new Guid("5eb344ef-9e7a-4a78-8e3e-23b11f1ed65a"));

            migrationBuilder.DeleteData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: new Guid("ddc2be4a-bad6-4602-b34f-2a7b9b5f1502"));

            migrationBuilder.DeleteData(
                table: "ConcertDates",
                keyColumn: "Id",
                keyValue: new Guid("7af48f46-bb1c-4efa-bfe9-37c57de10c00"));

            migrationBuilder.DeleteData(
                table: "ConcertDates",
                keyColumn: "Id",
                keyValue: new Guid("a9f9d5ac-9fe2-4141-a1a1-e7e2dba34e4f"));

            migrationBuilder.DeleteData(
                table: "ConcertDates",
                keyColumn: "Id",
                keyValue: new Guid("faccd7a9-7bab-47a8-9300-296b34a8809e"));

            migrationBuilder.DeleteData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: new Guid("111d640b-63d2-42bc-b5a6-79f77463d4fd"));

            migrationBuilder.DeleteData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: new Guid("4e9c45c0-a41a-473a-88ad-ef4276bb5301"));

            migrationBuilder.DeleteData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("a0ef757c-4fae-46b4-8b20-a0ebc4b9eb7e"));

            migrationBuilder.RenameColumn(
                name: "LinkedReservationId",
                table: "PromoCodes",
                newName: "UsedByReservationId");

            migrationBuilder.AddColumn<Guid>(
                name: "GeneratedByReservationId",
                table: "PromoCodes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AppSettings",
                columns: new[] { "Id", "Key", "Value" },
                values: new object[,]
                {
                    { new Guid("31ac573d-cae8-46a3-94c5-b27f7312f6d6"), "EarlyBirdDiscountPercentage", "10" },
                    { new Guid("39120002-0361-4c5d-8843-7b8776a88bdd"), "FriendPromoDiscountPercentage", "5" },
                    { new Guid("50b4a649-1e8b-4b89-9b14-f5f8ff15fa5e"), "EarlyBirdDiscountDaysBefore", "60" },
                    { new Guid("a902686e-36a0-45d2-b334-867bfeff5d52"), "FifthTicketDiscountPercentage", "50" }
                });

            migrationBuilder.InsertData(
                table: "Concerts",
                columns: new[] { "Id", "AdditionalInfo", "Address", "City", "Name", "Venue" },
                values: new object[] { new Guid("e874b9e5-6272-4611-8770-b16bfe197985"), "Open air concert", "Piazza del Colosseo, 1", "Rome", "Eros Ramazzotti Live", "Colosseum" });

            migrationBuilder.InsertData(
                table: "Zones",
                columns: new[] { "Id", "Capacity", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("019fc76d-c9b1-4708-a516-eaafcb32896d"), 500, "Regular", 100.00m },
                    { new Guid("e1e62828-6848-43f7-a9b3-2523ec61401e"), 100, "VIP", 250.00m }
                });

            migrationBuilder.InsertData(
                table: "ConcertDates",
                columns: new[] { "Id", "ConcertId", "Date" },
                values: new object[,]
                {
                    { new Guid("058736ee-98e1-4238-afc5-9beaf73e4a8f"), new Guid("e874b9e5-6272-4611-8770-b16bfe197985"), new DateTime(2027, 11, 15, 21, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("4a7d1d9e-15fc-42da-9fe3-9d6c3fc4f852"), new Guid("e874b9e5-6272-4611-8770-b16bfe197985"), new DateTime(2025, 11, 19, 21, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6ab859ea-cef3-4762-9c17-181c6aa1bab3"), new Guid("e874b9e5-6272-4611-8770-b16bfe197985"), new DateTime(2026, 11, 17, 21, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_GeneratedByReservationId",
                table: "PromoCodes",
                column: "GeneratedByReservationId",
                unique: true,
                filter: "[GeneratedByReservationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_UsedByReservationId",
                table: "PromoCodes",
                column: "UsedByReservationId",
                unique: true,
                filter: "[UsedByReservationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Reservations_GeneratedByReservationId",
                table: "PromoCodes",
                column: "GeneratedByReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Reservations_UsedByReservationId",
                table: "PromoCodes",
                column: "UsedByReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Reservations_GeneratedByReservationId",
                table: "PromoCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Reservations_UsedByReservationId",
                table: "PromoCodes");

            migrationBuilder.DropIndex(
                name: "IX_PromoCodes_GeneratedByReservationId",
                table: "PromoCodes");

            migrationBuilder.DropIndex(
                name: "IX_PromoCodes_UsedByReservationId",
                table: "PromoCodes");

            migrationBuilder.DeleteData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: new Guid("31ac573d-cae8-46a3-94c5-b27f7312f6d6"));

            migrationBuilder.DeleteData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: new Guid("39120002-0361-4c5d-8843-7b8776a88bdd"));

            migrationBuilder.DeleteData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: new Guid("50b4a649-1e8b-4b89-9b14-f5f8ff15fa5e"));

            migrationBuilder.DeleteData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: new Guid("a902686e-36a0-45d2-b334-867bfeff5d52"));

            migrationBuilder.DeleteData(
                table: "ConcertDates",
                keyColumn: "Id",
                keyValue: new Guid("058736ee-98e1-4238-afc5-9beaf73e4a8f"));

            migrationBuilder.DeleteData(
                table: "ConcertDates",
                keyColumn: "Id",
                keyValue: new Guid("4a7d1d9e-15fc-42da-9fe3-9d6c3fc4f852"));

            migrationBuilder.DeleteData(
                table: "ConcertDates",
                keyColumn: "Id",
                keyValue: new Guid("6ab859ea-cef3-4762-9c17-181c6aa1bab3"));

            migrationBuilder.DeleteData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: new Guid("019fc76d-c9b1-4708-a516-eaafcb32896d"));

            migrationBuilder.DeleteData(
                table: "Zones",
                keyColumn: "Id",
                keyValue: new Guid("e1e62828-6848-43f7-a9b3-2523ec61401e"));

            migrationBuilder.DeleteData(
                table: "Concerts",
                keyColumn: "Id",
                keyValue: new Guid("e874b9e5-6272-4611-8770-b16bfe197985"));

            migrationBuilder.DropColumn(
                name: "GeneratedByReservationId",
                table: "PromoCodes");

            migrationBuilder.RenameColumn(
                name: "UsedByReservationId",
                table: "PromoCodes",
                newName: "LinkedReservationId");

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
                name: "IX_Reservations_UsedPromoCodeId",
                table: "Reservations",
                column: "UsedPromoCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_LinkedReservationId",
                table: "PromoCodes",
                column: "LinkedReservationId",
                unique: true,
                filter: "[LinkedReservationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Reservations_LinkedReservationId",
                table: "PromoCodes",
                column: "LinkedReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_PromoCodes_UsedPromoCodeId",
                table: "Reservations",
                column: "UsedPromoCodeId",
                principalTable: "PromoCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
