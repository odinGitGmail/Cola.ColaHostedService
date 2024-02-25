using Cola.Core.Models.ColaBgServiceModels;
using Microsoft.Extensions.Hosting;

namespace Cola.ColaHostedService.ColaBgJob
{
    /// <summary>
    /// ColaBackgroundRecurringJob -  循环任务执行：通过间隔时间和延迟时间重复执行的任务
    /// </summary>
    public class ColaBackgroundRecurringJob : BackgroundService
    {
        private Timer? _timer;
        private readonly TimeSpan _period;
        private readonly TimeSpan _dueTime;
        private readonly Action<CancellationToken>? _startAsyncAction;
        private readonly Action<object>? _executeAsyncAction;
        private readonly Object? _state;
        private readonly Action<CancellationToken>? _stopAsyncAction;
        private readonly Action? _disposeAction;
        public ColaBackgroundRecurringJob(ColaRecurringJobModel options)
        {
            _period = options.Period;
            _dueTime = options.DueTime;
            _startAsyncAction = options.StartAsyncAction;
            _executeAsyncAction = options.ExecuteAsyncAction;
            _stopAsyncAction = options.StopAsyncAction;
            _disposeAction = options.DisposeAction;
            _state = options.State;
        }
        private void RecurringJobWorker(object state)
        {
            _executeAsyncAction!(state);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _startAsyncAction!(cancellationToken);
            return ExecuteAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(RecurringJobWorker!, _state, _dueTime, _period);
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _stopAsyncAction!(cancellationToken);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            _disposeAction!.Invoke();
        }
    }
}