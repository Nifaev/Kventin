using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kventin.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MarkType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MarkType",
                table: "Marks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkType",
                table: "Marks");
        }
    }
}
