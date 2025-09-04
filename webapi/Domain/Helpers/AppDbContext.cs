using Microsoft.EntityFrameworkCore;
using webapi.Domain.Models;
//using webapi.Domain.Models;

namespace webapi.Domain.Helpers
{
    public class AppDbContext : DbContext
    {
        public DbSet<HdCategories> HdCategories { get; set; }
        public DbSet<HdChannels> HdChannels { get; set; }
        public DbSet<HdComments> HdComments { get; set; }
        public DbSet<HdCompanies> HdCompanies { get; set; }
        public DbSet<HdDepartments> HdDepartments { get; set; }
        public DbSet<HdFileAttachments> HdFileAttachments { get; set; }
        public DbSet<HdLevels> HdLevels { get; set; }
        public DbSet<HdRequests> HdRequests { get; set; }
        public DbSet<HdLogs> HdLogs { get; set; }
        public DbSet<HdStatus> HdStatuses { get; set; }
        public DbSet<HdSubCategories> HdSubCategories { get; set; }
        public DbSet<HdTickets> HdTickets { get; set; }
        public DbSet<HdUsers> HdUsers { get; set; }
        public DbSet<HdEscalation> HdEscalation { get; set; }
        public DbSet<HdEscalationTimers> HdEscalationTimers { get; set; }
        public DbSet<SmtpSettings> SmtpSettings { get; set; }
        public DbSet<EscalationProfile> EscalationProfiles { get; set; }
        public DbSet<EscalationLevel> EscalationLevels { get; set; }
        public DbSet<EscalationMapping> EscalationMappings { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin");
            var agentHashedPassword = BCrypt.Net.BCrypt.HashPassword("agent");
            var superHashedPassword = BCrypt.Net.BCrypt.HashPassword("supervisor");
            var backHashedPassword = BCrypt.Net.BCrypt.HashPassword("backoffice");
            var currentDateTimeUtc = DateTime.UtcNow;
            var adminUser = new HdUsers
            {
                Id = -1,
                User_Id = "1",
                Username = "admin",
                Email = "admin.ae@vocalcom.com",
                Password = hashedPassword,
                IsAdministrator = true,
                IsSuperVisor = false,
                IsAgent = false,
                IsBackOffice = false,
                Firstname = "Administrator",
                Lastname = "Hermes",
                Phone = "+97144464100",
                Disabled = false,
                LastSeen = currentDateTimeUtc,
                IPAddress = "127.0.0.1",
                HostName = "locahost",
                Department_Id = "0",
                Department_Name = "0",
                LastPasswordChange = currentDateTimeUtc,
                LastLogoutDate = currentDateTimeUtc,
                Status = "Offline",
                Created_at = currentDateTimeUtc,
                Updated_at = currentDateTimeUtc,
                Deleted = false
            };
            var agentUser = new HdUsers
            {
                Id = -2,
                User_Id = "2",
                Username = "agent",
                Email = "agent.ae@vocalcom.com",
                Password = agentHashedPassword,
                IsAdministrator = false,
                IsSuperVisor = false,
                IsAgent = true,
                IsBackOffice = false,
                Firstname = "Agent",
                Lastname = "Hermes",
                Phone = "+97144464100",
                Disabled = false,
                LastSeen = currentDateTimeUtc,
                IPAddress = "127.0.0.1",
                HostName = "locahost",
                Department_Id = "0",
                Department_Name = "0",
                LastPasswordChange = currentDateTimeUtc,
                LastLogoutDate = currentDateTimeUtc,
                Status = "Offline",
                Created_at = currentDateTimeUtc,
                Updated_at = currentDateTimeUtc,
                Deleted = false
            };
            var superUser = new HdUsers
            {
                Id = -3,
                User_Id = "3",
                Username = "supervisor",
                Email = "supervisor.ae@vocalcom.com",
                Password = superHashedPassword,
                IsAdministrator = false,
                IsSuperVisor = true,
                IsAgent = false,
                IsBackOffice = false,
                Firstname = "Supervisor",
                Lastname = "Hermes",
                Phone = "+97144464100",
                Disabled = false,
                LastSeen = currentDateTimeUtc,
                IPAddress = "127.0.0.1",
                HostName = "locahost",
                Department_Id = "0",
                Department_Name = "0",
                LastPasswordChange = currentDateTimeUtc,
                LastLogoutDate = currentDateTimeUtc,
                Status = "Offline",
                Created_at = currentDateTimeUtc,
                Updated_at = currentDateTimeUtc,
                Deleted = false
            };
            var backOfficeUser = new HdUsers
            {
                Id = -4,
                User_Id = "4",
                Username = "backoffice",
                Email = "backoffice.ae@vocalcom.com",
                Password = backHashedPassword,
                IsAdministrator = false,
                IsSuperVisor = false,
                IsAgent = false,
                IsBackOffice = true,
                Firstname = "Back-Office",
                Lastname = "Hermes",
                Phone = "+97144464100",
                Disabled = false,
                LastSeen = currentDateTimeUtc,
                IPAddress = "127.0.0.1",
                HostName = "locahost",
                Department_Id = "0",
                Department_Name = "0",
                LastPasswordChange = currentDateTimeUtc,
                LastLogoutDate = currentDateTimeUtc,
                Status = "Offline",
                Created_at = currentDateTimeUtc,
                Updated_at = currentDateTimeUtc,
                Deleted = false
            };
            builder.Entity<HdUsers>().HasData(adminUser);
            builder.Entity<HdUsers>().HasData(backOfficeUser);
            builder.Entity<HdUsers>().HasData(agentUser);
            builder.Entity<HdUsers>().HasData(superUser);

            var escalationTimer = new HdEscalationTimers
            {
                Id = 1,
                Days=0,
                Hours = 8,
                Minutes=0
            };
            builder.Entity<HdEscalationTimers>().HasData(escalationTimer);
            var newStatus = new HdStatus
            {
                StatusID = 1,
                Description = "New",

            };
            var openStatus = new HdStatus
            {
                StatusID = 2,
                Description = "In progress",

            };
            var ClosedStatus = new HdStatus
            {
                StatusID = 3,
                Description = "Closed",

            };
            var resolvedStatus = new HdStatus
            {
                StatusID = 4,
                Description = "Resolved",
            };
            builder.Entity<HdStatus>().HasData(newStatus);
            builder.Entity<HdStatus>().HasData(openStatus);
            builder.Entity<HdStatus>().HasData(ClosedStatus);
            builder.Entity<HdStatus>().HasData(resolvedStatus);

            var lowPriority = new HdLevels
            {
                LevelID = 1,
                Description = "Low",

            };
            var midPriority = new HdLevels
            {
                LevelID = 2,
                Description = "Medium",

            };
            var highPriority = new HdLevels
            {
                LevelID = 3,
                Description = "High",

            };
            builder.Entity<HdLevels>().HasData(lowPriority);
            builder.Entity<HdLevels>().HasData(midPriority);
            builder.Entity<HdLevels>().HasData(highPriority);

            var company = new HdCompanies
            {
                CompanyID = 1,
                Description = "VOCALCOM AE",

            };
            builder.Entity<HdCompanies>().HasData(company);

            // Helpful index to speed up duplicate checks (non-unique, safe)
            builder.Entity<HdTickets>()
                .HasIndex(t => new { t.DepartmentID, t.RequestID, t.Mobile, t.StatusID })
                .HasDatabaseName("IX_HdTickets_DupCheck");

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }
    }
}


