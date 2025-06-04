using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class _2ndMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), "$2a$10$cvQIvf97DhcNlQfVZaj12.KGJK68yHGtFXnkX3KqNVPOdeDP1WAx.", new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), "$2a$10$vnpw1O5skzazrUK4HRgvJOAVaL8grvFzwJW.gGWfkv97Wq88pztiy", new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), "$2a$10$lElAnHdghUMAASJ8Tfl0te2CMg05CxlIHfSrLz3nGNkhHIXVn/hDS", new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), "$2a$10$7dWkIfMM.O7ug1SH//3epOQtlBJNzLdlgRQf7zINCtmXDAmE5oUCe", new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848) });
        }
    }
}
