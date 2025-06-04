using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using webapi.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using webapi.Domain.Helpers;
using System.Timers;

public class EmailEscalationBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;


    public EmailEscalationBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var _emailEscalationService = scope.ServiceProvider.GetRequiredService<IEmailEscalationService>();
                _emailEscalationService.ProcessEscalations();
                var timer = dbContext.HdEscalationTimers.FirstOrDefault();
                await Task.Delay(TimeSpan.FromHours(timer!.Hours), stoppingToken);
            }
        }
    }
}
