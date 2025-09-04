using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using webapi.Domain.Services;
using webapi.Domain.Models;
using webapi.Domain.Helpers;

public class EmailEscalationBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EmailEscalationBackgroundService> _logger;
    private readonly TimeSpan _defaultDelay = TimeSpan.FromMinutes(30); // fallback interval

    public EmailEscalationBackgroundService(IServiceScopeFactory scopeFactory, ILogger<EmailEscalationBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[Escalation] Background service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var emailEscalationService = scope.ServiceProvider.GetRequiredService<IEmailEscalationService>();

                _logger.LogDebug("[Escalation] Executing escalation processing cycle at {Now}", DateTime.Now);
                await emailEscalationService.ProcessEscalationsAsync(stoppingToken);
                _logger.LogDebug("[Escalation] Escalation processing completed");

                var delay = dbContext.HdEscalationTimers.FirstOrDefault()?.ToTimeSpan() ?? _defaultDelay;
                _logger.LogInformation("[Escalation] Waiting for next cycle in {Minutes} minutes", delay.TotalMinutes);

                await Task.Delay(delay, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Escalation] Error in background service loop, retrying in {Minutes} minutes", _defaultDelay.TotalMinutes);
                await Task.Delay(_defaultDelay, stoppingToken);
            }
        }

        _logger.LogWarning("[Escalation] Background service cancellation requested");
    }

}

