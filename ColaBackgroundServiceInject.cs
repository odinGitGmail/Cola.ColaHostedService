using Cola.ColaHostedService.ColaBgJob;
using Cola.Core.Models.ColaBgServiceModels;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.ColaHostedService
{
    public static class ColaBackgroundServiceInject
    {
        /// <summary>
        /// 后台任务 - 自定义任务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddColaBgServiceJob(this IServiceCollection services, Action<ColaBgJobModel> options)
        {
            var opt = new ColaBgJobModel();
            options(opt);
            services.AddHostedService<ColaBackgroundJob>(
                provider => new ColaBackgroundJob(opt));
            return services;
        }
        
        /// <summary>
        /// 后台任务 - 循环任务执行：重复执行的任务(任务执行完后继续自动执行)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddColaBgServiceLoopJob(this IServiceCollection services, Action<ColaLoopJobModel> options)
        {
            var opt = new ColaLoopJobModel();
            options(opt);
            services.AddHostedService<ColaBackgroundLoopJob>(
                provider => new ColaBackgroundLoopJob(opt));
            return services;
        }

        /// <summary>
        /// 后台任务 - 普通任务，立即执行，只执行一次
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddColaBgServiceNormalJob(this IServiceCollection services, Action<ColaNormalJobModel> options)
        {
            services.AddSingleton<ColaBackgroundNormalJob>();
            return services;
        }

        /// <summary>
        /// 后台任务 - 循环任务执行：重复执行的任务，使用常见的时间循环模式
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddColaBgServiceRecurringJob(this IServiceCollection services, Action<ColaRecurringJobModel> options)
        {
            var opt = new ColaRecurringJobModel();
            options(opt);
            services.AddHostedService<ColaBackgroundRecurringJob>(
                provider => new ColaBackgroundRecurringJob(opt));
            return services;
        }

        
    }
}