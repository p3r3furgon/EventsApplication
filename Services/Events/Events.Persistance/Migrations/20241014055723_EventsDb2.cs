using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class EventsDb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventEntityParticipantEntity");

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Participants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDateTime",
                table: "Participants",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Participants_EventId",
                table: "Participants",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Events_EventId",
                table: "Participants",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Events_EventId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_EventId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "RegistrationDateTime",
                table: "Participants");

            migrationBuilder.CreateTable(
                name: "EventEntityParticipantEntity",
                columns: table => new
                {
                    EventsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventEntityParticipantEntity", x => new { x.EventsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_EventEntityParticipantEntity_Events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventEntityParticipantEntity_Participants_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventEntityParticipantEntity_ParticipantsId",
                table: "EventEntityParticipantEntity",
                column: "ParticipantsId");
        }
    }
}
