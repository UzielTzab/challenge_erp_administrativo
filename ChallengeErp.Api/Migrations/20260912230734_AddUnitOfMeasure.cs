using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChallengeErp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitOfMeasure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitOfMeasure",
                table: "OrderLines",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "OrderLines",
                keyColumn: "Id",
                keyValue: 1,
                column: "UnitOfMeasure",
                value: "m2");

            migrationBuilder.UpdateData(
                table: "OrderLines",
                keyColumn: "Id",
                keyValue: 2,
                column: "UnitOfMeasure",
                value: "pza");

            migrationBuilder.UpdateData(
                table: "OrderLines",
                keyColumn: "Id",
                keyValue: 3,
                column: "UnitOfMeasure",
                value: "pza");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitOfMeasure",
                table: "OrderLines");
        }
    }
}
