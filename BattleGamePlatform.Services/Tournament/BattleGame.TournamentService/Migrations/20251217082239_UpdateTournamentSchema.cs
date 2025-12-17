using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleGame.TournamentService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTournamentSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "Tournaments"
                ALTER COLUMN "GameId"
                TYPE uuid
                USING "GameId"::uuid;
            """);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "Tournaments"
                ALTER COLUMN "GameId"
                TYPE text;
            """);

        }
    }
}
