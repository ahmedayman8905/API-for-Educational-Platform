using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_1.Migrations
{
    /// <inheritdoc />
    public partial class Edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserToken<string>",
                table: "IdentityUserToken<string>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserRole<string>",
                table: "IdentityUserRole<string>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserLogin<string>",
                table: "IdentityUserLogin<string>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserClaim<string>",
                table: "IdentityUserClaim<string>");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityRoleClaim<string>",
                table: "IdentityRoleClaim<string>");

            migrationBuilder.RenameTable(
                name: "IdentityUserToken<string>",
                newName: "IdentityUserToken");

            migrationBuilder.RenameTable(
                name: "IdentityUserRole<string>",
                newName: "IdentityUserRole");

            migrationBuilder.RenameTable(
                name: "IdentityUserLogin<string>",
                newName: "IdentityUserLogin");

            migrationBuilder.RenameTable(
                name: "IdentityUserClaim<string>",
                newName: "IdentityUserClaim");

            migrationBuilder.RenameTable(
                name: "IdentityRoleClaim<string>",
                newName: "IdentityRoleClaim");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserToken",
                table: "IdentityUserToken",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserRole",
                table: "IdentityUserRole",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserLogin",
                table: "IdentityUserLogin",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserClaim",
                table: "IdentityUserClaim",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRoleClaim",
                table: "IdentityRoleClaim",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserToken",
                table: "IdentityUserToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserRole",
                table: "IdentityUserRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserLogin",
                table: "IdentityUserLogin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUserClaim",
                table: "IdentityUserClaim");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityRoleClaim",
                table: "IdentityRoleClaim");

            migrationBuilder.RenameTable(
                name: "IdentityUserToken",
                newName: "IdentityUserToken<string>");

            migrationBuilder.RenameTable(
                name: "IdentityUserRole",
                newName: "IdentityUserRole<string>");

            migrationBuilder.RenameTable(
                name: "IdentityUserLogin",
                newName: "IdentityUserLogin<string>");

            migrationBuilder.RenameTable(
                name: "IdentityUserClaim",
                newName: "IdentityUserClaim<string>");

            migrationBuilder.RenameTable(
                name: "IdentityRoleClaim",
                newName: "IdentityRoleClaim<string>");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserToken<string>",
                table: "IdentityUserToken<string>",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserRole<string>",
                table: "IdentityUserRole<string>",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserLogin<string>",
                table: "IdentityUserLogin<string>",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUserClaim<string>",
                table: "IdentityUserClaim<string>",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityRoleClaim<string>",
                table: "IdentityRoleClaim<string>",
                column: "Id");
        }
    }
}
