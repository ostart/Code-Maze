using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyEmployees.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedRolesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0dbb7758-a725-442e-994d-912140b45605", null, "Administrator", "ADMINISTRATOR" },
                    { "b71d64eb-1be2-48b2-a251-497b2538e861", null, "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0dbb7758-a725-442e-994d-912140b45605");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b71d64eb-1be2-48b2-a251-497b2538e861");
        }
    }
}
