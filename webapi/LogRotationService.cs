using log4net.Config;

namespace webapi
{
    public class LogRotationService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Implement your log rotation and archival logic here

                // Update the log4net configuration file with the new log file path
                XmlConfigurator.Configure(new FileInfo("log4net.config"));

                await Task.Delay(TimeSpan.FromHours(8), stoppingToken); // Delay for 8 hours
            }
        }
    }
}
