using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391.Migrations
{
    /// <inheritdoc />
    public partial class Update_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingResults_Bookings_BookingId1",
                table: "BookingResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Bookings_BookingId1",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_BookingId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_BookingId1",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_BookingResults_BookingId",
                table: "BookingResults");

            migrationBuilder.DropIndex(
                name: "IX_BookingResults_BookingId1",
                table: "BookingResults");

            migrationBuilder.DropColumn(
                name: "BookingId1",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "BookingId1",
                table: "BookingResults");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_BookingId",
                table: "Feedbacks",
                column: "BookingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingResults_BookingId",
                table: "BookingResults",
                column: "BookingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_BookingId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_BookingResults_BookingId",
                table: "BookingResults");

            migrationBuilder.AddColumn<Guid>(
                name: "BookingId1",
                table: "Feedbacks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BookingId1",
                table: "BookingResults",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_BookingId",
                table: "Feedbacks",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_BookingId1",
                table: "Feedbacks",
                column: "BookingId1",
                unique: true,
                filter: "[BookingId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BookingResults_BookingId",
                table: "BookingResults",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingResults_BookingId1",
                table: "BookingResults",
                column: "BookingId1",
                unique: true,
                filter: "[BookingId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingResults_Bookings_BookingId1",
                table: "BookingResults",
                column: "BookingId1",
                principalTable: "Bookings",
                principalColumn: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Bookings_BookingId1",
                table: "Feedbacks",
                column: "BookingId1",
                principalTable: "Bookings",
                principalColumn: "BookingId");
        }
    }
}
