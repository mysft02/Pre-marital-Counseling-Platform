using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391.Migrations
{
    /// <inheritdoc />
    public partial class Update_Enum_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuizStatus",
                table: "Quizes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuizStatus",
                table: "Quizes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Categories");
        }
    }
}
