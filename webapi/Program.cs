
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
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var postgresConnectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
    
    // Use PostgreSQL if available (for Render), otherwise fall back to SQL Server
    if (!string.IsNullOrEmpty(postgresConnectionString))
    {
        options.UseNpgsql(postgresConnectionString);
    }
    else
    {
        options.UseSqlServer(connectionString);
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

// Only run migrations if database connection is available
try
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
}
catch (Exception ex)
{
    // Log the error but don't fail startup
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogWarning(ex, "Database migration failed during startup. This is normal if database is not yet configured.");
}
app.UseWhen(context => context.Request.Path.StartsWithSegments("/swagger"), appBuilder =>
{
    appBuilder.UseMiddleware<BasicAuthMiddleware>();
});
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
        var logFilePath = "/logs/startup.log";
        File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
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
app.MapGet("/health", () => "OK");

app.MapControllers();

// Log that the app is ready
logger.LogInformation("H360 Helpdesk API is ready and listening on port 80");

app.Run();
