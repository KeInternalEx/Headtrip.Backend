using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System.Diagnostics;

namespace Headtrip.UeService
{
    internal class Program
    {
        /*
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IJobFactory, JobFactory>();
                    services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
                    services.AddSingleton<QuartzJobRunner>();
                    services.AddScoped<Job1>();

                    services.AddSingleton<Service1>();
                    //services.AddScoped<Service1>();
                    //services.AddTransient<Service1>();

                    services.AddHostedService<QuartzHostedService>();
                });
        public static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);

            if (Debugger.IsAttached)
            {
                hostBuilder.Build().Run();
            }
            else
            {
                hostBuilder.RunAsTopshelfService(hc =>
                {
                    hc.SetDisplayName("Task1");
                    hc.SetDescription("Task1");
                    hc.SetServiceName("Task1");
                });
            }
        }

        */

        

        static void Main(string[] args)
        {





            // todo: start topshelf service
            // todo: split on super UeService flag
            // todo: if we're the super UeService, we run the ServerTransferRequest transformation task
            // todo: if we're not a super UeService, then we manage the server spinning tasks


        }
    }
}