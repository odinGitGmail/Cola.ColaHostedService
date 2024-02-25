using Cola.Core.Models.ColaBgServiceModels;
using Microsoft.Extensions.Hosting;

namespace Cola.ColaHostedService.ColaBgJob
{
    /// <summary>
    /// ColaBackgroundJob
    /// </summary>
    public class ColaBackgroundJob : BackgroundService
    {
        private readonly Action<CancellationToken>? _startAsyncAction;
        private readonly Action<CancellationToken>? _executeAsyncAction;
        private readonly Action<CancellationToken>? _stopAsyncAction;
        private readonly Action? _disposeAction;
        public ColaBackgroundJob(ColaBgJobModel options)
        {
            _startAsyncAction = options.StartAsyncAction;
            _executeAsyncAction = options.ExecuteAsyncAction;
            _stopAsyncAction = options.StopAsyncAction;
            _disposeAction = options.DisposeAction;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _startAsyncAction!(cancellationToken);
            return ExecuteAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _executeAsyncAction?.Invoke(cancellationToken);
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _stopAsyncAction!(cancellationToken);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _disposeAction?.Invoke();
        }
    }
}