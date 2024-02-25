using Cola.Core.Models.ColaBgServiceModels;
using Microsoft.Extensions.Hosting;

namespace Cola.ColaHostedService.ColaBgJob
{
    /// <summary>
    /// ColaBackgroundNormalJob - 普通任务，立即执行，只执行一次
    /// </summary>
    public class ColaBackgroundNormalJob : BackgroundService
    {
        public Func<CancellationToken,Task>? StartFuncAsync { get; set; }
        public Func<CancellationToken,Task>? ExecuteFuncAsync { get; set; }
        public Func<CancellationToken,Task>? StopFuncAsync { get; set; }
        public Action? DisposeAction { get; set; }
        public Func<CancellationToken,Task>? NormalJobWorkerFuncAsync { get; set; }
        
        private void NormalJobWorkerAsync(CancellationToken cancellationToken)
        {
            NormalJobWorkerFuncAsync?.Invoke(cancellationToken);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            StartFuncAsync?.Invoke(cancellationToken);
            return ExecuteAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            ExecuteFuncAsync?.Invoke(cancellationToken);
            NormalJobWorkerAsync(cancellationToken);
            return StopAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            StopFuncAsync?.Invoke(cancellationToken);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            DisposeAction?.Invoke();
            GC.SuppressFinalize(this);
        }
    }
}