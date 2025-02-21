using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391.Migrations
{
    /// <inheritdoc />
    public partial class Update_Therapist_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Therapists",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Therapists",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Therapists");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Therapists");
        }
    }
}
