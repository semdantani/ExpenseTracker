using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddRecurringExpenseUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecurringExpenseId",
                table: "Expenses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_RecurringExpenseId_Date",
                table: "Expenses",
                columns: new[] { "RecurringExpenseId", "Date" },
                unique: true,
                filter: "[RecurringExpenseId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_RecurringExpenses_RecurringExpenseId",
                table: "Expenses",
                column: "RecurringExpenseId",
                principalTable: "RecurringExpenses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_RecurringExpenses_RecurringExpenseId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_RecurringExpenseId_Date",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "RecurringExpenseId",
                table: "Expenses");
        }
    }
}
