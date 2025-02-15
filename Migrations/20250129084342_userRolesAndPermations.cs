using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api_1.Migrations
{
    /// <inheritdoc />
    public partial class userRolesAndPermations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "92b75286-d8f8-4061-9995-e6e23ccdee94", "f51e5a91-bced-49c2-8b86-c2e170c0846c", false, false, "Admin", "ADMIN" },
                    { "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4", "5ee6bc12-5cb0-4304-91e7-6a00744e042a", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "IdentityRoleClaim",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "updateStudent", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 2, "permissions", "getstudents", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 3, "permissions", "DeleteStudent", "92b75286-d8f8-4061-9995-e6e23ccdee94" },
                    { 4, "permissions", "addstudent", "92b75286-d8f8-4061-9995-e6e23ccdee94" }
                });

            migrationBuilder.InsertData(
                table: "IdentityUserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "92b75286-d8f8-4061-9995-e6e23ccdee94", "6dc6528a-b280-4770-9eae-82671ee81ef7" });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "Id", "AccessFailedCount", "BirthDay", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "Gender", "IsDelete", "JoinDate", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Password", "PasswordHash", "Phone", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6dc6528a-b280-4770-9eae-82671ee81ef7", 0, null, "99d2bbc6-bc54-4248-a172-a77de3ae4430", "admin@gmail.com", true, "Admin", "male", false, new DateTime(2025, 1, 29, 8, 43, 41, 466, DateTimeKind.Utc).AddTicks(9335), false, null, "ADMIN@GMAIL.COM", "ADMIN@GMAIL.COM", "P@ssword123", "AQAAAAIAAYagAAAAEJKQTwW5IboasThNoLPHbZcEDB8Bh/InZZaZjJ6SXespb/19B0KNv6EuNdvgEU31aw==", null, null, false, "55BF92C9EF0249CDA210D85D1A851BC9", false, "admin@gmail.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92b75286-d8f8-4061-9995-e6e23ccdee94");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9eaa03df-8e4f-4161-85de-0f6e5e30bfd4");

            migrationBuilder.DeleteData(
                table: "IdentityRoleClaim",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "IdentityRoleClaim",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "IdentityRoleClaim",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "IdentityRoleClaim",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "IdentityUserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "92b75286-d8f8-4061-9995-e6e23ccdee94", "6dc6528a-b280-4770-9eae-82671ee81ef7" });

            migrationBuilder.DeleteData(
                table: "Student",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AspNetRoles",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDefault",
                table: "AspNetRoles",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");
        }
    }
}
