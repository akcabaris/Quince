using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApplicationLLA.Server.Migrations
{
    /// <inheritdoc />
    public partial class PostPriceValueChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c1c6f20-c5a7-4364-af02-846c22ffc48d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6d4e92d-d00c-46b6-950a-112c6ce7c2f0");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PriceCurrency",
                table: "Posts",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PriceWorkUnit",
                table: "Posts",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3212787f-32e1-47d4-9f70-02056fe13471", null, "Customer", "CUSTOMER" },
                    { "8248fb1d-0a53-4fa3-b727-bf259d749bb2", null, "Worker", "WORKER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3212787f-32e1-47d4-9f70-02056fe13471");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8248fb1d-0a53-4fa3-b727-bf259d749bb2");

            migrationBuilder.DropColumn(
                name: "PriceCurrency",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PriceWorkUnit",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "Price",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c1c6f20-c5a7-4364-af02-846c22ffc48d", null, "Customer", "CUSTOMER" },
                    { "e6d4e92d-d00c-46b6-950a-112c6ce7c2f0", null, "Worker", "WORKER" }
                });
        }
    }
}
