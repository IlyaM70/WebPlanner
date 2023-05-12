using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPlanner.Migrations
{
    /// <inheritdoc />
    public partial class removeCommetraries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Commentary",
                table: "ToDos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Commentary",
                table: "ToDos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
