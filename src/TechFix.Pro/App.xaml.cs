using Microsoft.UI.Xaml;
using Microsoft.Extensions.DependencyInjection;
using TechFix.Pro.Data.Context;
using TechFix.Pro.Services.DeviceDetection;
using TechFix.Pro.Services.Logging;
using TechFix.Pro.Services.FirmwareManagement;
using TechFix.Pro.Services.DriverManagement;
using TechFix.Pro.Services.ToolsManagement;
using Serilog;

namespace TechFix.Pro
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;
        public static Window MainWindow { get; set; } = null!;

        public App()
        {
            this.InitializeComponent();
            ConfigureServices();
            ConfigureLogging();
        }

        private void OnLaunched(LaunchActivatedEventArgs args)
        {
            MainWindow = new MainWindow();
            MainWindow.Activate();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Data Layer
            services.AddScoped<TechFixDbContext>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IFirmwareRepository, FirmwareRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IToolRepository, ToolRepository>();

            // Services
            services.AddScoped<IDeviceDetectionService, DeviceDetectionService>();
            services.AddScoped<ILoggingService, LoggingService>();
            services.AddScoped<IFirmwareService, FirmwareService>();
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<IToolsService, ToolsService>();

            Services = services.BuildServiceProvider();
        }

        private void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TechFix Pro", "logs", "app-.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}