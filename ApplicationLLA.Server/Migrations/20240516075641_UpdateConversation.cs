using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApplicationLLA.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "155504e5-d1a5-4792-bb67-c25ee870ed07");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "54165d2b-52a8-4334-813f-3c51f5fd33f9");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastReadDateFirstUser",
                table: "Conversations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastReadDateSecondUser",
                table: "Conversations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "NumberOfUnreadMessages",
                table: "Conversations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a3d20adc-ff44-4444-a620-a48b184c0242", null, "Worker", "WORKER" },
                    { "f48dc4e6-f2e6-4f65-8b80-a1b12de13438", null, "Customer", "CUSTOMER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3d20adc-ff44-4444-a620-a48b184c0242");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f48dc4e6-f2e6-4f65-8b80-a1b12de13438");

            migrationBuilder.DropColumn(
                name: "LastReadDateFirstUser",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "LastReadDateSecondUser",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "NumberOfUnreadMessages",
                table: "Conversations");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "155504e5-d1a5-4792-bb67-c25ee870ed07", null, "Worker", "WORKER" },
                    { "54165d2b-52a8-4334-813f-3c51f5fd33f9", null, "Customer", "CUSTOMER" }
                });
        }
    }
}
