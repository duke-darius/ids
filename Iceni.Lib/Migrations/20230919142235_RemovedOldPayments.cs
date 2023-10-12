using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iceni.Lib.Migrations
{
    /// <inheritdoc />
    public partial class RemovedOldPayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentRecords");

            migrationBuilder.DropTable(
                name: "LessonPaymentInformation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LessonPaymentInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CostGross = table.Column<decimal>(type: "money", nullable: false),
                    DateSettled = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Settled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonPaymentInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonPaymentInformation_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonPaymentInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AmountPaid = table.Column<decimal>(type: "money", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentRecords_LessonPaymentInformation_LessonPaymentInformationId",
                        column: x => x.LessonPaymentInformationId,
                        principalTable: "LessonPaymentInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonPaymentInformation_LessonId",
                table: "LessonPaymentInformation",
                column: "LessonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRecords_LessonPaymentInformationId",
                table: "PaymentRecords",
                column: "LessonPaymentInformationId");
        }
    }
}
