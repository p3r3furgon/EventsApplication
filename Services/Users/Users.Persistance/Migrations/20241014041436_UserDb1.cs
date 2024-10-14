using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UserDb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b0f6021f-6f9e-4f6f-a7d1-234fa2ef1342"),
                column: "PasswordHash",
                value: "$2a$11$Xv32BiMyYMrwRoI/cV59GeCbQDkLD.P7ilftAV.Izj1w1hVWxhfgS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserRole",
                table: "RefreshTokens",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b0f6021f-6f9e-4f6f-a7d1-234fa2ef1342"),
                column: "PasswordHash",
                value: "$2a$11$/aTz.7ZLc2AUcFuCiylE9O1OfMf2mgL.7Cgns6VSFHS9zNuhZV0Gm");
        }
    }
}
