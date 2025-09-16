
using DinkToPdf.Contracts;
using DinkToPdf;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;
using webapi;
using webapi.Domain.Helpers;
using webapi.Domain.Models;
using webapi.Domain.Services;
using webapi.Domain.Config;
using Microsoft.Extensions.Logging;

log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));

var builder = WebApplication.CreateBuilder(args);

// Add log4net integration
builder.Logging.ClearProviders();
builder.Logging.AddLog4Net("log4net.config");

// Ensure log directories exist
Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "logs/Controllers"));
Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "logs/Startup"));
Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "logs/Angular"));


builder.Services.AddControllers();
builder.Services.Configure<ExternalApisOptions>(builder.Configuration.GetSection("ExternalApis"));
builder.Services.Configure<EscalationSettings>(builder.Configuration.GetSection("Escalation"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Set up basic authentication for Swagger
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",  // This specifies that we're using HTTP Basic Authentication
        In = ParameterLocation.Header,
        Description = "Basic Authentication header"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddHttpClient();
builder.Services.AddCors();
builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddTransient<SmsService>();
builder.Services.AddScoped<SmtpSettings>();
builder.Services.AddScoped<SmtpSettings>();
builder.Services.AddHostedService<EmailEscalationBackgroundService>();
builder.Services.AddHostedService<LogRotationService>();
builder.Services.AddTransient<IEmailEscalationService, EmailEscalationService>();
builder.Services.AddTransient<EmailService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    // Check for Railway PostgreSQL connection string
    var postgresConnectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
    
    // Log connection string for debugging
    Console.WriteLine($"PostgreSQLConnection: {postgresConnectionString}");
    
    // Check if we have a valid PostgreSQL connection string
    var hasValidPostgres = !string.IsNullOrEmpty(postgresConnectionString) && 
                          postgresConnectionString.StartsWith("postgresql://");
    
    Console.WriteLine($"Has valid PostgreSQL connection: {hasValidPostgres}");
    
    if (hasValidPostgres)
    {
        // Parse the PostgreSQL connection string
        var uri = new Uri(postgresConnectionString);
        var host = uri.Host;
        var port = uri.Port;
        var database = uri.AbsolutePath.TrimStart('/');
        var username = uri.UserInfo.Split(':')[0];
        var password = uri.UserInfo.Split(':')[1];
        
        var npgsqlConnectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};";
        
        Console.WriteLine("Configuring Railway PostgreSQL database");
        Console.WriteLine($"Host: {host}, Port: {port}, Database: {database}, Username: {username}");
        options.UseNpgsql(npgsqlConnectionString);
    }
    else
    {
        // Fall back to SQL Server for local development
        var sqlServerConnection = "Server=VCLLAECD2370YYT;Database=H360_Helpdesk;Trusted_Connection=True;TrustServerCertificate=True";
        Console.WriteLine("Configuring SQL Server database (local development)");
        options.UseSqlServer(sqlServerConnection);
    }
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TqszqFDnFdoQXhbPSi1H4gGZFj853KNUwtXPBfunXxs="))
    };
});
var app = builder.Build();

// Log startup information
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("H360 Helpdesk API starting up...");

// Run database migrations on startup
logger.LogInformation("Starting database migration process...");
try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();
        
        logger.LogInformation("Running database migrations...");
        
        // Check if database exists
        var canConnect = context.Database.CanConnect();
        logger.LogInformation($"Can connect to database: {canConnect}");
        
        if (canConnect)
        {
            // Get pending migrations
            var pendingMigrations = context.Database.GetPendingMigrations();
            logger.LogInformation($"Pending migrations count: {pendingMigrations.Count()}");
            logger.LogInformation($"Pending migrations: {string.Join(", ", pendingMigrations)}");
            
            // Apply migrations
            logger.LogInformation("Applying migrations...");
            try
            {
                context.Database.Migrate();
                logger.LogInformation("Database migrations completed successfully.");
            }
            catch (Exception migrationEx)
            {
                logger.LogError(migrationEx, "Migration failed, attempting to create database: {Message}", migrationEx.Message);
                
                // Fallback: Create database if migrations fail
                try
                {
                    logger.LogInformation("Creating database...");
                    context.Database.EnsureCreated();
                    logger.LogInformation("Database created successfully using EnsureCreated()");
                }
                catch (Exception createEx)
                {
                    logger.LogError(createEx, "Failed to create database: {Message}", createEx.Message);
                }
            }
            
            // Verify tables exist
            try
            {
                var tables = context.Database.GetDbConnection().GetSchema("Tables");
                logger.LogInformation($"Database tables: {string.Join(", ", tables.Rows.Cast<System.Data.DataRow>().Select(r => r["TABLE_NAME"]))}");
            }
            catch (Exception tableEx)
            {
                logger.LogWarning($"Could not list tables: {tableEx.Message}");
            }
        }
        else
        {
            logger.LogError("Cannot connect to database, skipping migrations");
        }
    }
}
catch (Exception ex)
{
    logger.LogError(ex, "Database migration failed during startup: {Message}", ex.Message);
    // Don't fail startup, but log the error
}
// Temporarily disable basic auth for Swagger to allow Railway health checks
// app.UseWhen(context => context.Request.Path.StartsWithSegments("/swagger"), appBuilder =>
// {
//     appBuilder.UseMiddleware<BasicAuthMiddleware>();
// });
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.DefaultModelsExpandDepth(-1);
//    });
//}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "swagger"; // Changed to just "swagger" for easier health check
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VOCALCOM MEA Ticketing API V1");
    //c.DefaultModelsExpandDepth(-1);
});


app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;
        var logger = LogManager.GetLogger(typeof(Program));
        logger.Error($"Unexpected error: {exception}");
        var logEntry = $"[{DateTime.Now}] Unexpected error: {exception}";
        var logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs", "startup.log");
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }
        catch (Exception logEx)
        {
            Console.WriteLine($"Failed to write to log file: {logEx.Message}");
        }
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("An unexpected error occurred.");
        Console.WriteLine($"Unexpected error: {exception}");
    });
});
app.UseHttpsRedirection();
app.UseCors(cfg => cfg.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthorization();

// Add a simple health check endpoint
app.MapGet("/health", () => {
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Health check endpoint called");
    return "OK";
});

// Add a simple status endpoint for debugging
app.MapGet("/status", () => {
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Status endpoint called");
    return new { 
        status = "running", 
        timestamp = DateTime.UtcNow,
        environment = app.Environment.EnvironmentName
    };
});

// Add a Railway-compatible health check endpoint
app.MapGet("/", () => {
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Root endpoint called (Railway health check)");
    return new { 
        status = "OK", 
        message = "H360 Helpdesk API is running",
        timestamp = DateTime.UtcNow
    };
});

app.MapControllers();

// Log that the app is ready
var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
logger.LogInformation($"H360 Helpdesk API is ready and listening on port {port}");

app.Run($"http://0.0.0.0:{port}");
