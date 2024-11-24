using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RegistrationModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventRegistration_Events_EventId",
                table: "EventRegistration");

            migrationBuilder.DropForeignKey(
                name: "FK_EventRegistration_Users_UserId",
                table: "EventRegistration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventRegistration",
                table: "EventRegistration");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "EventRegistration");

            migrationBuilder.RenameTable(
                name: "EventRegistration",
                newName: "Registrations");

            migrationBuilder.RenameIndex(
                name: "IX_EventRegistration_UserId",
                table: "Registrations",
                newName: "IX_Registrations_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EventRegistration_EventId",
                table: "Registrations",
                newName: "IX_Registrations_EventId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Registrations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registrations",
                table: "Registrations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Events_EventId",
                table: "Registrations",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Users_UserId",
                table: "Registrations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Events_EventId",
                table: "Registrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Users_UserId",
                table: "Registrations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Registrations",
                table: "Registrations");

            migrationBuilder.RenameTable(
                name: "Registrations",
                newName: "EventRegistration");

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_UserId",
                table: "EventRegistration",
                newName: "IX_EventRegistration_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Registrations_EventId",
                table: "EventRegistration",
                newName: "IX_EventRegistration_EventId");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "EventRegistration",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "EventRegistration",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventRegistration",
                table: "EventRegistration",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventRegistration_Events_EventId",
                table: "EventRegistration",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EventRegistration_Users_UserId",
                table: "EventRegistration",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
