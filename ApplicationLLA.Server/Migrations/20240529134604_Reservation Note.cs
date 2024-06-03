using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApplicationLLA.Server.Migrations
{
    /// <inheritdoc />
    public partial class ReservationNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4212121-966e-4aa2-80d3-0973bd18a188");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3fdb7ce-d45f-4472-80c7-3d3a96143972");

            migrationBuilder.AddColumn<string>(
                name: "ReservationNote",
                table: "Reservations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "19469183-e844-41ce-9a55-94d7241c7046", null, "Worker", "WORKER" },
                    { "ee0f9e3f-366f-4507-bdcf-9821676944a3", null, "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19469183-e844-41ce-9a55-94d7241c7046");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee0f9e3f-366f-4507-bdcf-9821676944a3");

            migrationBuilder.DropColumn(
                name: "ReservationNote",
                table: "Reservations");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d4212121-966e-4aa2-80d3-0973bd18a188", null, "Customer", "CUSTOMER" },
                    { "e3fdb7ce-d45f-4472-80c7-3d3a96143972", null, "Worker", "WORKER" }
                });
        }
    }
}
