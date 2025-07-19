using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class EscalationsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EscalationLevels",
                columns: table => new
                {
                    LevelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscalationLevels", x => x.LevelID);
                });

            migrationBuilder.CreateTable(
                name: "EscalationMappings",
                columns: table => new
                {
                    MappingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentID = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    SubcategoryID = table.Column<int>(type: "int", nullable: false),
                    PriorityID = table.Column<int>(type: "int", nullable: false),
                    Level1ProfileID = table.Column<int>(type: "int", nullable: false),
                    Level2ProfileID = table.Column<int>(type: "int", nullable: true),
                    Level3ProfileID = table.Column<int>(type: "int", nullable: true),
                    Level1Delay = table.Column<TimeSpan>(type: "time", nullable: true),
                    Level2Delay = table.Column<TimeSpan>(type: "time", nullable: true),
                    Level3Delay = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscalationMappings", x => x.MappingID);
                });

            migrationBuilder.CreateTable(
                name: "EscalationProfiles",
                columns: table => new
                {
                    ProfileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    EscalationLevelLevelID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscalationProfiles", x => x.ProfileID);
                    table.ForeignKey(
                        name: "FK_EscalationProfiles_EscalationLevels_EscalationLevelLevelID",
                        column: x => x.EscalationLevelLevelID,
                        principalTable: "EscalationLevels",
                        principalColumn: "LevelID");
                });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), "$2a$10$jpttcGtdzZzpOrgC6al7x.7VM62LGUzfP3./SebpZez7hRnDndtNS", new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), "$2a$10$3ql8wZwi.bxpLL5Oek3WXek7JzxSFee628Y5Olt00gykowOUcBUVO", new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), "$2a$10$ia.lWQtWs091cADg9SKos.xpUTKt5yKOJW8KJ51OLINSBJAvhQzly", new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358), "$2a$10$72lgLiCgVQOoUjKaUc/P8.VMQwH5Il46IlR8VsHjL0r08Ryael9a.", new DateTime(2025, 7, 17, 15, 38, 29, 317, DateTimeKind.Utc).AddTicks(8358) });

            migrationBuilder.CreateIndex(
                name: "IX_EscalationProfiles_EscalationLevelLevelID",
                table: "EscalationProfiles",
                column: "EscalationLevelLevelID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EscalationMappings");

            migrationBuilder.DropTable(
                name: "EscalationProfiles");

            migrationBuilder.DropTable(
                name: "EscalationLevels");

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), "$2a$10$Rp2/Az.uMLRXgSrXj4ueqe7aut5eePbhiBzl5w49Ylss4IHjsDbJW", new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), "$2a$10$kB3ED6aX.kJrsQ0494jUEekQXTwQiZUKq.zt5qZxNLVgBwtXsdlMm", new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), "$2a$10$C9KyCD0/g23JUDqa6VbAHOrguDRLaBZiL1PBq8gOQ9dQpyuAOBKkW", new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756) });

            migrationBuilder.UpdateData(
                table: "HdUsers",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Created_at", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Password", "Updated_at" },
                values: new object[] { new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756), "$2a$10$iLUqXX8ibrILLTXLl32Xt..LBA1rr0EOsKel6J.vP6zC60kRIO69O", new DateTime(2025, 7, 17, 13, 0, 41, 743, DateTimeKind.Utc).AddTicks(9756) });
        }
    }
}
