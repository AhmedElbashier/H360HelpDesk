using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class _5thMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "SmtpSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "SmtpSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentReply",
                table: "HdTickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), "$2a$10$ml/TOB3mamLlzZXXmLNHgeHg0YtTzrE6XwNnwQEvXl89810piUao.", new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), "$2a$10$pHwk54q7NQMnKMrYC3s..uz6Ufif/y.kAg.nk6EThkqHfl5W4sdom", new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), "$2a$10$Ek/fTRPnSA0HeurObo9eFujF0Foj6BirIAoyrgs5rIZRMWj6fBwZO", new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867), "$2a$10$DB0KW6BWSxC3COjS5kBNMume7rppVNQEKILkvbPkchxsRDIW3dJvq", new DateTime(2023, 10, 22, 8, 21, 53, 834, DateTimeKind.Utc).AddTicks(7867) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentReply",
                table: "HdTickets");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "SmtpSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "SmtpSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
