using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class EnsureSuperAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b0f6021f-6f9e-4f6f-a7d1-234fa2ef1342"),
                column: "PasswordHash",
                value: "$2a$11$01hYmBuqwuFjb.lFWSnlZebI4HOotkdunos3giu2csqgvSnNf6jtm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b0f6021f-6f9e-4f6f-a7d1-234fa2ef1342"),
                column: "PasswordHash",
                value: "$2a$11$G/vj9l.oVHKBMkU1ky4s/u4l3OG5i4dMDvermOa6eNyiqQKf/6VOW");
        }
    }
}
