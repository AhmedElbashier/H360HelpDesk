using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class _4thMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssingedToBackOfficeID",
                table: "HdTickets",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), "$2a$10$tHHAwxjMnDlbh.f0uPaHo.RpcSQkSjPSKa.z5WMPfZyT2YQ/jVYDC", new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), "$2a$10$gKAXCb73kfKAMQskiJaISeWGo6bwWRToU0ZzKWRSzMb0e57.xh.u.", new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), "$2a$10$Fr/yKaXTZGdPDlG/OYM5LOId4XyUp5ihvv.lBFcDikzH6N7btRO1O", new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648), "$2a$10$eoFdlaShjV5Z6eiTDu8cl.tfuQczztcx.yVQowh9k5GUKysrJHm2m", new DateTime(2023, 10, 14, 22, 14, 4, 980, DateTimeKind.Utc).AddTicks(5648) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssingedToBackOfficeID",
                table: "HdTickets");

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
    }
}
