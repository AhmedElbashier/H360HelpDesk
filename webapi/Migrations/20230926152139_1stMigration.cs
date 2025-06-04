using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class _1stMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HdCategories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdCategories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "HdChannels",
                columns: table => new
                {
                    ChannelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdChannels", x => x.ChannelID);
                });

            migrationBuilder.CreateTable(
                name: "HdComments",
                columns: table => new
                {
                    CommentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketID = table.Column<int>(type: "int", nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TicketFlag = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdComments", x => x.CommentID);
                });

            migrationBuilder.CreateTable(
                name: "HdCompanies",
                columns: table => new
                {
                    CompanyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdCompanies", x => x.CompanyID);
                });

            migrationBuilder.CreateTable(
                name: "HdDepartments",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdDepartments", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "HdEscalation",
                columns: table => new
                {
                    EscalationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentID = table.Column<int>(type: "int", nullable: false),
                    levelID = table.Column<int>(type: "int", nullable: false),
                    Days = table.Column<int>(type: "int", nullable: false),
                    Emails = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdEscalation", x => x.EscalationID);
                });

            migrationBuilder.CreateTable(
                name: "HdFileAttachments",
                columns: table => new
                {
                    FileID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketID = table.Column<int>(type: "int", nullable: false),
                    CommentID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdFileAttachments", x => x.FileID);
                });

            migrationBuilder.CreateTable(
                name: "HdLevels",
                columns: table => new
                {
                    LevelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdLevels", x => x.LevelID);
                });

            migrationBuilder.CreateTable(
                name: "HdLogs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdLogs", x => x.LogID);
                });

            migrationBuilder.CreateTable(
                name: "HdRequests",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdRequests", x => x.RequestID);
                });

            migrationBuilder.CreateTable(
                name: "HdStatuses",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "HdSubCategories",
                columns: table => new
                {
                    SubCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdSubCategories", x => x.SubCategoryID);
                });

            migrationBuilder.CreateTable(
                name: "HdTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    Indice = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    SubCategoryID = table.Column<int>(type: "int", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false),
                    ChannelID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolvedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssingedToUserID = table.Column<int>(type: "int", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    RequestID = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    EscalationLevel = table.Column<int>(type: "int", nullable: false),
                    UpdateByUser = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SMSAlert = table.Column<bool>(type: "bit", nullable: false),
                    EmailAlert = table.Column<bool>(type: "bit", nullable: false),
                    Flag = table.Column<bool>(type: "bit", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdTickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HdUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HostName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Department_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdministrator = table.Column<bool>(type: "bit", nullable: false),
                    IsSuperVisor = table.Column<bool>(type: "bit", nullable: false),
                    IsAgent = table.Column<bool>(type: "bit", nullable: false),
                    IsBackOffice = table.Column<bool>(type: "bit", nullable: false),
                    Disabled = table.Column<bool>(type: "bit", nullable: false),
                    DarkMode = table.Column<bool>(type: "bit", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastPasswordChange = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogoutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HdUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmtpSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UseSsl = table.Column<bool>(type: "bit", nullable: false),
                    UseDefaultCredentials = table.Column<bool>(type: "bit", nullable: false),
                    FromAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmtpSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HdUsers",
                columns: new[] { "Id", "Created_at", "DarkMode", "Deleted", "Department_Id", "Department_Name", "Disabled", "Email", "Firstname", "HostName", "IPAddress", "IsAdministrator", "IsAgent", "IsBackOffice", "IsSuperVisor", "LastLogoutDate", "LastPasswordChange", "LastSeen", "Lastname", "Password", "Phone", "Status", "Updated_at", "User_Id", "Username" },
                values: new object[,]
                {
                    { -4, new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), false, false, "0", "0", false, "backoffice.ae@vocalcom.com", "Back-Office", "locahost", "127.0.0.1", false, false, true, false, new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), "Hermes", "$2a$10$cvQIvf97DhcNlQfVZaj12.KGJK68yHGtFXnkX3KqNVPOdeDP1WAx.", "+97144464100", "Offline", new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), "4", "backoffice" },
                    { -3, new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), false, false, "0", "0", false, "supervisor.ae@vocalcom.com", "Supervisor", "locahost", "127.0.0.1", false, false, false, true, new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), "Hermes", "$2a$10$vnpw1O5skzazrUK4HRgvJOAVaL8grvFzwJW.gGWfkv97Wq88pztiy", "+97144464100", "Offline", new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), "3", "supervisor" },
                    { -2, new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), false, false, "0", "0", false, "agent.ae@vocalcom.com", "Agent", "locahost", "127.0.0.1", false, true, false, false, new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), "Hermes", "$2a$10$lElAnHdghUMAASJ8Tfl0te2CMg05CxlIHfSrLz3nGNkhHIXVn/hDS", "+97144464100", "Offline", new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), "2", "agent" },
                    { -1, new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), false, false, "0", "0", false, "admin.ae@vocalcom.com", "Administrator", "locahost", "127.0.0.1", true, false, false, false, new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), "Hermes", "$2a$10$7dWkIfMM.O7ug1SH//3epOQtlBJNzLdlgRQf7zINCtmXDAmE5oUCe", "+97144464100", "Offline", new DateTime(2023, 9, 26, 15, 21, 39, 480, DateTimeKind.Utc).AddTicks(3848), "1", "admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HdCategories");

            migrationBuilder.DropTable(
                name: "HdChannels");

            migrationBuilder.DropTable(
                name: "HdComments");

            migrationBuilder.DropTable(
                name: "HdCompanies");

            migrationBuilder.DropTable(
                name: "HdDepartments");

            migrationBuilder.DropTable(
                name: "HdEscalation");

            migrationBuilder.DropTable(
                name: "HdFileAttachments");

            migrationBuilder.DropTable(
                name: "HdLevels");

            migrationBuilder.DropTable(
                name: "HdLogs");

            migrationBuilder.DropTable(
                name: "HdRequests");

            migrationBuilder.DropTable(
                name: "HdStatuses");

            migrationBuilder.DropTable(
                name: "HdSubCategories");

            migrationBuilder.DropTable(
                name: "HdTickets");

            migrationBuilder.DropTable(
                name: "HdUsers");

            migrationBuilder.DropTable(
                name: "SmtpSettings");
        }
    }
}
