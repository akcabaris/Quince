using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApplicationLLA.Server.Migrations
{
    /// <inheritdoc />
    public partial class PhoneUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3d20adc-ff44-4444-a620-a48b184c0242");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f48dc4e6-f2e6-4f65-8b80-a1b12de13438");

            migrationBuilder.AlterColumn<long>(
                name: "PhoneNumber",
                table: "Workers",
                type: "bigint",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d4212121-966e-4aa2-80d3-0973bd18a188", null, "Customer", "CUSTOMER" },
                    { "e3fdb7ce-d45f-4472-80c7-3d3a96143972", null, "Worker", "WORKER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4212121-966e-4aa2-80d3-0973bd18a188");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3fdb7ce-d45f-4472-80c7-3d3a96143972");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Workers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 15);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a3d20adc-ff44-4444-a620-a48b184c0242", null, "Worker", "WORKER" },
                    { "f48dc4e6-f2e6-4f65-8b80-a1b12de13438", null, "Customer", "CUSTOMER" }
                });
        }
    }
}
