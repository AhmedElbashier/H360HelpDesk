using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class SupervisorDepartmentMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDepartmentRestrictedSupervisor",
                table: "HdUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Created_at", "IsDepartmentRestrictedSupervisor", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), false, new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), "$2a$10$Rp2/Az.uMLRXgSrXj4ueqe7aut5eePbhiBzl5w49Ylss4IHjsDbJW", new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Created_at", "IsDepartmentRestrictedSupervisor", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), false, new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), "$2a$10$kB3ED6aX.kJrsQ0494jUEekQXTwQiZUKq.zt5qZxNLVgBwtXsdlMm", new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Created_at", "IsDepartmentRestrictedSupervisor", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), false, new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), "$2a$10$C9KyCD0/g23JUDqa6VbAHOrguDRLaBZiL1PBq8gOQ9dQpyuAOBKkW", new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Created_at", "IsDepartmentRestrictedSupervisor", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), false, new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), "$2a$10$iLUqXX8ibrILLTXLl32Xt..LBA1rr0EOsKel6J.vP6zC60kRIO69O", new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDepartmentRestrictedSupervisor",
                table: "HdUsers");

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), "$2a$10$YeiGoSlKih5a2IHLH9EAweIFDHlVzN5klS4LQ4qXZxN/..snCE8i2", new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), "$2a$10$tMMbByVkk2f7tvmMX8HtduXnJmcdPLW1SJ1zsXE.p3xktbqSAzY8O", new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), "$2a$10$BL24u9XexvNlHy5GVrjbv.6uuSG3RWjFWQmi0aYi7GJGVFL/AUxyq", new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473), "$2a$10$rwljqVOs3T9IRqhiMtLZyOxUz9o1FA2NMk9eUVYt0e8T/N5UwKzrq", new DateTime(2025, 6, 4, 12, 0, 28, 98, DateTimeKind.Utc).AddTicks(2473) });
        }
    }
}
