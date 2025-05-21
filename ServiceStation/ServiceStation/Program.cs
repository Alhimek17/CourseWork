using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServiceStationContracts.BusinessLogicsContracts;
using ServiceStationDatabaseImplement.Implements;
using NLog.Extensions.Logging;
using ServiceStationBusinessLogic.BusinessLogics;
using ServiceStationContracts.StoragesContracts;


namespace ServiceStation
{
    internal static class Program
    {
        private static ServiceProvider? _serviceProvider;
        public static ServiceProvider? ServiceProvider => _serviceProvider;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
            Application.Run(_serviceProvider.GetRequiredService<Form1>());
        }
        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(option =>
            {
                option.SetMinimumLevel(LogLevel.Information);
                option.AddNLog("nlog.config");
            });
            services.AddTransient<IExecutorStorage, ExecutorStorage>();
            services.AddTransient<IGuarantorStorage, GuarantorStorage>();
            services.AddTransient<ICarStorage, CarStorage>();
            services.AddTransient<IDefectStorage, DefectStorage>();
            services.AddTransient<IRepairStorage, RepairStorage>();
            services.AddTransient<ISparePartStorage, SparePartStorage>();
            services.AddTransient<ITechnicalWorkStorage, TechnicalWorkStorage>();
            services.AddTransient<IWorkStorage, WorkStorage>();

            services.AddTransient<IExecutorLogic, ExecutorLogic>();
            services.AddTransient<IGuarantorLogic, GuarantorLogic>();
            services.AddTransient<ICarLogic, CarLogic>();
            services.AddTransient<IDefectLogic, DefectLogic>();
            services.AddTransient<IRepairLogic, RepairLogic>();
            services.AddTransient<ISparePartLogic, SparePartLogic>();
            services.AddTransient<ITechnicalWorkLogic, TechnicalWorkLogic>();
            services.AddTransient<IWorkLogic, WorkLogic>();

            services.AddTransient<Form1>();
        }
    }
}