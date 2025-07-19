
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

log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));

var builder = WebApplication.CreateBuilder(args);


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
    DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
    if (!optionsBuilder.IsConfigured)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
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
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
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
    c.RoutePrefix = "api-docs"; // Ensure this matches your expected path
    c.SwaggerEndpoint("/ticketing-api/swagger/v1/swagger.json", "VOCALCOM MEA Ticketing API V1");
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
app.MapControllers();
app.Run();
