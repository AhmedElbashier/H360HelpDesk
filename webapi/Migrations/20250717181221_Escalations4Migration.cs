using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class Escalations4Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimerID",
                table: "HdEscalationTimers",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Hours",
                table: "HdEscalationTimers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Days",
                table: "HdEscalationTimers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Minutes",
                table: "HdEscalationTimers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "HdEscalationTimers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Days", "Minutes" },
                values: new object[] { 0, 0 });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), "$2a$10$klBlQlELgFQMP58TmGHyveLMiVqjkyxwZapxx7lZHvWwso4QB3Ik2", new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), "$2a$10$oeDiVp4P3YwLZDnr9uO5NOTz0O6IyZpBjUyJ5E3fwyfNTTt0Dm4Fu", new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), "$2a$10$RMBd/f1OjlcsR8AZIQtZb.vX/EoXkY509tyBRCHQFREA5.mDrsgye", new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836), "$2a$10$E59f1AbP/ZGCbNAzB.TxeunwRT38XFyxZPR/A2UpVZ7D6D4hgh7ue", new DateTime(2025, 7, 17, 18, 12, 21, 481, DateTimeKind.Utc).AddTicks(3836) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Days",
                table: "HdEscalationTimers");

            migrationBuilder.DropColumn(
                name: "Minutes",
                table: "HdEscalationTimers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "HdEscalationTimers",
                newName: "TimerID");

            migrationBuilder.AlterColumn<int>(
                name: "Hours",
                table: "HdEscalationTimers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), "$2a$10$s4c2YfX.NawYHkbGkCL5RuYRn/4FFN4IKjnjR42BPV69dWbE9bVvi", new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), "$2a$10$A6./5UGQgY3tWWObwNQ2vOS/EyI/7Kgjpbr5h9dIs6OVP7ZDOBCku", new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), "$2a$10$3O5T/oHS4OgJA736xurfCeCeW2/sHASOtvcGdJu.ZijoZQ5A0JOji", new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779), "$2a$10$OtpZqvFlEvb6wnWkbpV7desp93UQqUp/cDpBbLw.jrsM7AYzrnowC", new DateTime(2025, 7, 17, 17, 29, 48, 91, DateTimeKind.Utc).AddTicks(3779) });
        }
    }
}
