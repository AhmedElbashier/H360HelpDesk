using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class Escalations2Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EscalationProfiles_EscalationLevels_EscalationLevelLevelID",
                table: "EscalationProfiles");

            migrationBuilder.DropIndex(
                name: "IX_EscalationProfiles_EscalationLevelLevelID",
                table: "EscalationProfiles");

            migrationBuilder.DropColumn(
                name: "EscalationLevelLevelID",
                table: "EscalationProfiles");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "EscalationProfiles",
                newName: "EscalationLevelID");

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

            migrationBuilder.CreateIndex(
                name: "IX_EscalationProfiles_EscalationLevelID",
                table: "EscalationProfiles",
                column: "EscalationLevelID");

            migrationBuilder.AddForeignKey(
                name: "FK_EscalationProfiles_EscalationLevels_EscalationLevelID",
                table: "EscalationProfiles",
                column: "EscalationLevelID",
                principalTable: "EscalationLevels",
                principalColumn: "LevelID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EscalationProfiles_EscalationLevels_EscalationLevelID",
                table: "EscalationProfiles");

            migrationBuilder.DropIndex(
                name: "IX_EscalationProfiles_EscalationLevelID",
                table: "EscalationProfiles");

            migrationBuilder.RenameColumn(
                name: "EscalationLevelID",
                table: "EscalationProfiles",
                newName: "Level");

            migrationBuilder.AddColumn<int>(
                name: "EscalationLevelLevelID",
                table: "EscalationProfiles",
                type: "int",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_EscalationProfiles_EscalationLevels_EscalationLevelLevelID",
                table: "EscalationProfiles",
                column: "EscalationLevelLevelID",
                principalTable: "EscalationLevels",
                principalColumn: "LevelID");
        }
    }
}
