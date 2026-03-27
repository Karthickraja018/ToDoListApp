using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoApp.Migrations
{
    /// <inheritdoc />
    public partial class upgrade1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$7EqJtq98hPqEX7fNZaFWo.VpkS2w6QAIxWunpZqy5.lRzZgQGS8wi");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$7EqJtq98hPqEX7fNZaFWo.VpkS2w6QAIxWunpZqy5.lRzZgQGS8wi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$rXw8EDMoc5VoLcCVTYzf8egIcGcoxkyrMc.AidnV5ZNNI97RRhhGa");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$hPKFhyQdANvZY8ySmPNR1ugQy8XHtmcZFtVuGgv9t9HxBiQh3GHUO");
        }
    }
}
