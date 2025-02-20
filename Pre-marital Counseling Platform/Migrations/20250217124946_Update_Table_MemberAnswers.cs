using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWP391.Migrations
{
    /// <inheritdoc />
    public partial class Update_Table_MemberAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberAnswers",
                table: "MemberAnswers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberAnswers",
                table: "MemberAnswers",
                column: "MemberAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberAnswers_AnswerId",
                table: "MemberAnswers",
                column: "AnswerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberAnswers",
                table: "MemberAnswers");

            migrationBuilder.DropIndex(
                name: "IX_MemberAnswers_AnswerId",
                table: "MemberAnswers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberAnswers",
                table: "MemberAnswers",
                column: "AnswerId");
        }
    }
}
