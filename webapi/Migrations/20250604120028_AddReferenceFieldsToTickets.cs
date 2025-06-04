using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class AddReferenceFieldsToTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "HdTickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceType",
                table: "HdTickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            //migrationBuilder.InsertData(
            //    table: "HdCompanies",
            //    columns: new[] { "CompanyID", "Description" },
            //    values: new object[] { 1, "VOCALCOM AE" });

            //migrationBuilder.InsertData(
            //    table: "HdLevels",
            //    columns: new[] { "LevelID", "Description" },
            //    values: new object[,]
            //    {
            //        { 1, "Low" },
            //        { 2, "Medium" },
            //        { 3, "High" }
            //    });

            //migrationBuilder.InsertData(
            //    table: "HdStatuses",
            //    columns: new[] { "StatusID", "Description" },
            //    values: new object[,]
            //    {
            //        { 1, "New" },
            //        { 2, "In progress" },
            //        { 3, "Closed" },
            //        { 4, "Resolved" }
            //    });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HdCompanies",
                keyColumn: "CompanyID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "HdLevels",
                keyColumn: "LevelID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "HdLevels",
                keyColumn: "LevelID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "HdLevels",
                keyColumn: "LevelID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "HdStatuses",
                keyColumn: "StatusID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "HdStatuses",
                keyColumn: "StatusID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "HdStatuses",
                keyColumn: "StatusID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "HdStatuses",
                keyColumn: "StatusID",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "HdTickets");

            migrationBuilder.DropColumn(
                name: "ReferenceType",
                table: "HdTickets");

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
    }
}
