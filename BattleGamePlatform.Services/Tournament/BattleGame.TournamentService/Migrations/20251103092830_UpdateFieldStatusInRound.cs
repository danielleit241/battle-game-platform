using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleGame.TournamentService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFieldStatusInRound : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Rounds",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Rounds");
        }
    }
}
