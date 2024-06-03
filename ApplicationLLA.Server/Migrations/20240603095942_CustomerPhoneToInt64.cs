using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApplicationLLA.Server.Migrations
{
    /// <inheritdoc />
    public partial class CustomerPhoneToInt64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3212787f-32e1-47d4-9f70-02056fe13471");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8248fb1d-0a53-4fa3-b727-bf259d749bb2");

            migrationBuilder.AlterColumn<long>(
                name: "PhoneNumber",
                table: "Customer",
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
                    { "a012e5e8-f4f2-49dc-865a-3b205953f3fc", null, "Worker", "WORKER" },
                    { "e66a7b15-6f51-4f16-a89a-1c80b66c6e00", null, "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a012e5e8-f4f2-49dc-865a-3b205953f3fc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e66a7b15-6f51-4f16-a89a-1c80b66c6e00");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Customer",
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
                    { "3212787f-32e1-47d4-9f70-02056fe13471", null, "Customer", "CUSTOMER" },
                    { "8248fb1d-0a53-4fa3-b727-bf259d749bb2", null, "Worker", "WORKER" }
                });
        }
    }
}
