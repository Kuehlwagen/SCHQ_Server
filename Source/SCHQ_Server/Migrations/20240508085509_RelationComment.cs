using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SCHQ_Server.Migrations
{
    /// <inheritdoc />
    public partial class RelationComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Relations",
                type: "TEXT",
                nullable: true,
                collation: "NOCASE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Relations");
        }
    }
}
