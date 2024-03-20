using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoboAdvisorApp.API.Migrations
{
    /// <inheritdoc />
    public partial class Latestmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recommendations_QuestionnaireId",
                table: "Recommendations");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_QuestionnaireId",
                table: "Recommendations",
                column: "QuestionnaireId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recommendations_QuestionnaireId",
                table: "Recommendations");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_QuestionnaireId",
                table: "Recommendations",
                column: "QuestionnaireId",
                unique: true);
        }
    }
}
