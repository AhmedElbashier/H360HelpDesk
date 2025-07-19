using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class Escalations3Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), "$2a$10$btQA1/CfVJT9lPaklnenHecGP8CD5zXxPdKOF1G31ZUtgnpidvZVi", new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), "$2a$10$w8KtV6TWM/DdVH6YyClRvOSjLpyRR904/ItevjABW0n9qITULLqTW", new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), "$2a$10$iQZMyomdln2vziTsiluUbOaDsz.FxNeq7RKORvzTiSs0XHyxpPOj2", new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364), "$2a$10$ZOcNKHHYO/87NPnQE8NOI.HPJ/w489s3rhqny/kUrHBr1zD7tH4JO", new DateTime(2025, 7, 17, 17, 19, 2, 709, DateTimeKind.Utc).AddTicks(3364) });
        }
    }
}
