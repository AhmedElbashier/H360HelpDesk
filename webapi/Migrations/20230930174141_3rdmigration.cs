using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class _3rdmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HdEscalationTimers",
                columns: table => new
                {
                    TimerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdEscalationTimers", x => x.TimerID);
                });

            migrationBuilder.InsertData(
                table: "HdEscalationTimers",
                columns: new[] { "TimerID", "Hours" },
                values: new object[] { 1, 8 });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), "$2a$10$7Opj7bsIYkVCEKI53XWwR.HltFhw1f22ZIXY25cKLJ4Kmu5FMhMsS", new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), "$2a$10$PwHmPhoJN5UWK8qpqzA6D.j4vaVKeXEOwQkBlgWNQsuj5v86J4/Um", new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), "$2a$10$BYhvH5uJL5emvAahF9aWouEFYSexJ8kGbwvlqNkJuyhaNmfCvKsWS", new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374), "$2a$10$gspXgQ8f9huYLPZlkKpjvujDI0FLYsX/jhShMyewfjGALjCKf4Hnu", new DateTime(2023, 9, 30, 17, 41, 41, 855, DateTimeKind.Utc).AddTicks(4374) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HdEscalationTimers");

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), "$2a$10$fMljW35kM.qCsJjetp5/LuP5w2OcL9BbbfAlr2ZLtM/6.D7ZPs1n.", new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), "$2a$10$klvStRv4BMRCuZcKsYex9OI/u08.eT1hnIcaJ9stdoMRq1NV4g.MK", new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), "$2a$10$rL.9nAlOfQEECwz6aaoX2OYu.UJC0zrf2GaSCWpV9NOaXpbOUoImS", new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031), "$2a$10$YGoPR2GUot.aGA68z14KWeQTwZTGR25ZsWGhm652O3fxU8rhG6qoi", new DateTime(2023, 9, 27, 11, 57, 58, 279, DateTimeKind.Utc).AddTicks(3031) });
        }
    }
}
