using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using REAC.Application.Contracts.IAppServices.Integration;

namespace REAC.Application.AppServices.Integration.GEE
{
    public class NafathBackgroundTaskService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private static readonly object _lock = new();

        public NafathBackgroundTaskService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                lock (_lock)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var nafathService = scope.ServiceProvider.GetRequiredService<INafathService>();
                        if (nafathService is not null)
                            nafathService.CheckNafathStatus();
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}