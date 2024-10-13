using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notifications.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class NotificationsDb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageStatus",
                table: "Notifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MessageStatus",
                table: "Notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
