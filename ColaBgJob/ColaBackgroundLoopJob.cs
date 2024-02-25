using Cola.Core.Models.ColaBgServiceModels;
using Microsoft.Extensions.Hosting;

namespace Cola.ColaHostedService.ColaBgJob
{
    /// <summary>
    /// ColaBackgroundLoopJob - 循环任务执行：重复执行的任务(任务执行完后继续自动执行)
    /// </summary>
    public class ColaBackgroundLoopJob : BackgroundService
    {
        private readonly Action<CancellationToken>? _startAsyncAction;
        private readonly Action<CancellationToken>? _executeAsyncAction;
        private readonly Action? _disposeAction;

        public ColaBackgroundLoopJob(ColaLoopJobModel options)
        {
            _startAsyncAction = options.StartAsyncAction;
            _executeAsyncAction = options.ExecuteAsyncAction;
            _disposeAction = options.DisposeAction;
        }
        private async Task LoopJobWorkerAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() =>
                {
                    _executeAsyncAction!(cancellationToken);
                }, cancellationToken)
                .ContinueWith(
                    t => LoopJobWorkerAsync(cancellationToken), cancellationToken);

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _startAsyncAction!(cancellationToken);
            return ExecuteAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await LoopJobWorkerAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _disposeAction!.Invoke();
        }
    }
}