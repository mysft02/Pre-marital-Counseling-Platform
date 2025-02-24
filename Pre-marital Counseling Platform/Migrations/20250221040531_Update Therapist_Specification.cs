using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTherapist_Specification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TherapistSpecifications_TherapistId",
                table: "TherapistSpecifications");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TherapistSpecifications",
                table: "TherapistSpecifications",
                columns: new[] { "TherapistId", "SpecificationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TherapistSpecifications",
                table: "TherapistSpecifications");

            migrationBuilder.CreateIndex(
                name: "IX_TherapistSpecifications_TherapistId",
                table: "TherapistSpecifications",
                column: "TherapistId");
        }
    }
}
