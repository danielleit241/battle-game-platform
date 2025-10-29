using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleGame.UserService.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessedCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProcessedCount",
                table: "OutboxEvents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedCount",
                table: "OutboxEvents");
        }
    }
}
