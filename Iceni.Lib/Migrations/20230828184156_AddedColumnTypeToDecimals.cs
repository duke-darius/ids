using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iceni.Lib.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnTypeToDecimals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CostGross",
                table: "LessonPaymentInformation",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CostNet",
                table: "LessonPaymentInformation",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostGross",
                table: "LessonPaymentInformation");

            migrationBuilder.DropColumn(
                name: "CostNet",
                table: "LessonPaymentInformation");
        }
    }
}
