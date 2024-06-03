using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApplicationLLA.Server.Migrations
{
    /// <inheritdoc />
    public partial class WorkersPostsLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19469183-e844-41ce-9a55-94d7241c7046");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee0f9e3f-366f-4507-bdcf-9821676944a3");

            migrationBuilder.AddColumn<int>(
                name: "postLimit",
                table: "Workers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c1c6f20-c5a7-4364-af02-846c22ffc48d", null, "Customer", "CUSTOMER" },
                    { "e6d4e92d-d00c-46b6-950a-112c6ce7c2f0", null, "Worker", "WORKER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c1c6f20-c5a7-4364-af02-846c22ffc48d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6d4e92d-d00c-46b6-950a-112c6ce7c2f0");

            migrationBuilder.DropColumn(
                name: "postLimit",
                table: "Workers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "19469183-e844-41ce-9a55-94d7241c7046", null, "Worker", "WORKER" },
                    { "ee0f9e3f-366f-4507-bdcf-9821676944a3", null, "Customer", "CUSTOMER" }
                });
        }
    }
}
