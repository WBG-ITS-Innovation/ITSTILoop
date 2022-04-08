using ITSTILoop.Context.Repositories;

namespace ITSTILoop.Services
{
    public class TimedSettlementWindowService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedSettlementWindowService> _logger;
        private readonly ISettlementWindowRepository _settlementWindowRepository;
        private Timer _timer = null!;

        public TimedSettlementWindowService(ILogger<TimedSettlementWindowService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _settlementWindowRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ISettlementWindowRepository>();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(5),
                TimeSpan.FromDays(1.0));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);

            _settlementWindowRepository.CloseOpenSettlementWindow();
            _settlementWindowRepository.CreateNewSettlementWindow();
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
