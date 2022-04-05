namespace ITSTILoop.Services
{
    public class TimedSettlementWindowService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedSettlementWindowService> _logger;
        private Timer _timer = null!;

        public TimedSettlementWindowService(ILogger<TimedSettlementWindowService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            var tomorrow = DateTime.Now.Date.AddDays(1.0);
            var dueTime = tomorrow - DateTime.Now;

            _timer = new Timer(DoWork, null, dueTime,
                TimeSpan.FromDays(1.0));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
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
