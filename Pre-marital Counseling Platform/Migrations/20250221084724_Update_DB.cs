using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391.Migrations
{
    /// <inheritdoc />
    public partial class Update_DB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_MemberResults_MemberResultId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_MemberResultId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "MemberResultId",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MemberResultId",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_MemberResultId",
                table: "Bookings",
                column: "MemberResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_MemberResults_MemberResultId",
                table: "Bookings",
                column: "MemberResultId",
                principalTable: "MemberResults",
                principalColumn: "MemberResultId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
