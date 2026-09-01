namespace TechFix.Pro.ViewModels
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using TechFix.Pro.Core.Interfaces;
    using TechFix.Pro.Core.Models;
    using TechFix.Pro.Core.Enums;
    using System.Collections.ObjectModel;

    /// <summary>
    /// ViewModel for the Dashboard page
    /// </summary>
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly IDeviceDetectionService _deviceDetectionService;
        private readonly ILoggingService _loggingService;

        [ObservableProperty]
        private Device? connectedDevice;

        [ObservableProperty]
        private ObservableCollection<LogEntry> recentLogs = new();

        [ObservableProperty]
        private string deviceStatus = "Not Connected";

        [ObservableProperty]
        private bool isLoading = false;

        public DashboardViewModel(IDeviceDetectionService deviceDetectionService, ILoggingService loggingService)
        {
            _deviceDetectionService = deviceDetectionService;
            _loggingService = loggingService;
        }

        [RelayCommand]
        public async Task RefreshDeviceInfo()
        {
            IsLoading = true;
            try
            {
                ConnectedDevice = await _deviceDetectionService.DetectConnectedDeviceAsync();
                DeviceStatus = ConnectedDevice != null ? "Connected" : "Not Connected";

                var logs = await _loggingService.GetLogsAsync(10);
                RecentLogs = new ObservableCollection<LogEntry>(logs);
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error refreshing device info: {ex.Message}", LogLevel.Error, ex);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    /// <summary>
    /// ViewModel for Device Detection page
    /// </summary>
    public partial class DeviceDetectionViewModel : ObservableObject
    {
        private readonly IDeviceDetectionService _deviceDetectionService;
        private readonly ILoggingService _loggingService;

        [ObservableProperty]
        private ObservableCollection<Device> detectedDevices = new();

        [ObservableProperty]
        private Device? selectedDevice;

        [ObservableProperty]
        private bool isDetecting = false;

        public DeviceDetectionViewModel(IDeviceDetectionService deviceDetectionService, ILoggingService loggingService)
        {
            _deviceDetectionService = deviceDetectionService;
            _loggingService = loggingService;
        }

        [RelayCommand]
        public async Task ScanDevices()
        {
            IsDetecting = true;
            try
            {
                var devices = await _deviceDetectionService.DetectAllDevicesAsync();
                DetectedDevices = new ObservableCollection<Device>(devices);
                await _loggingService.LogAsync($"Scanned and found {devices.Count()} devices", LogLevel.Information);
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error scanning devices: {ex.Message}", LogLevel.Error, ex);
            }
            finally
            {
                IsDetecting = false;
            }
        }

        [RelayCommand]
        public async Task GetDeviceDetails()
        {
            if (SelectedDevice?.SerialNumber == null)
                return;

            try
            {
                var device = await _deviceDetectionService.GetDeviceInfoAsync(SelectedDevice.SerialNumber);
                if (device != null)
                {
                    SelectedDevice = device;
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error getting device details: {ex.Message}", LogLevel.Error, ex);
            }
        }
    }

    /// <summary>
    /// ViewModel for Firmware Manager page
    /// </summary>
    public partial class FirmwareViewModel : ObservableObject
    {
        private readonly IFirmwareService _firmwareService;
        private readonly ILoggingService _loggingService;

        [ObservableProperty]
        private ObservableCollection<Firmware> firmwares = new();

        [ObservableProperty]
        private Firmware? selectedFirmware;

        [ObservableProperty]
        private string searchQuery = string.Empty;

        [ObservableProperty]
        private bool isLoading = false;

        public FirmwareViewModel(IFirmwareService firmwareService, ILoggingService loggingService)
        {
            _firmwareService = firmwareService;
            _loggingService = loggingService;
        }

        [RelayCommand]
        public async Task LoadFirmwares()
        {
            IsLoading = true;
            try
            {
                var firmwares = await _firmwareService.GetAllFirmwaresAsync();
                Firmwares = new ObservableCollection<Firmware>(firmwares);
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error loading firmwares: {ex.Message}", LogLevel.Error, ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task AddFirmware(string filePath)
        {
            try
            {
                var success = await _firmwareService.AddFirmwareAsync(filePath);
                if (success)
                {
                    await LoadFirmwaresCommand.ExecuteAsync(null);
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error adding firmware: {ex.Message}", LogLevel.Error, ex);
            }
        }

        [RelayCommand]
        public async Task VerifyFirmware()
        {
            if (SelectedFirmware == null)
                return;

            try
            {
                var isValid = await _firmwareService.VerifyFirmwareIntegrityAsync(SelectedFirmware);
                var message = isValid ? "Firmware verified successfully" : "Firmware verification failed";
                await _loggingService.LogAsync(message, isValid ? LogLevel.Information : LogLevel.Warning);
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error verifying firmware: {ex.Message}", LogLevel.Error, ex);
            }
        }

        [RelayCommand]
        public async Task DeleteFirmware()
        {
            if (SelectedFirmware?.Id == null)
                return;

            try
            {
                var success = await _firmwareService.DeleteFirmwareAsync(SelectedFirmware.Id);
                if (success)
                {
                    await LoadFirmwaresCommand.ExecuteAsync(null);
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error deleting firmware: {ex.Message}", LogLevel.Error, ex);
            }
        }
    }

    /// <summary>
    /// ViewModel for Drivers page
    /// </summary>
    public partial class DriversViewModel : ObservableObject
    {
        private readonly IDriverService _driverService;
        private readonly ILoggingService _loggingService;

        [ObservableProperty]
        private ObservableCollection<Driver> installedDrivers = new();

        [ObservableProperty]
        private ObservableCollection<Driver> missingDrivers = new();

        [ObservableProperty]
        private Platform selectedPlatform = Platform.QualcommSnapdragon;

        [ObservableProperty]
        private bool isLoading = false;

        public DriversViewModel(IDriverService driverService, ILoggingService loggingService)
        {
            _driverService = driverService;
            _loggingService = loggingService;
        }

        [RelayCommand]
        public async Task LoadDrivers()
        {
            IsLoading = true;
            try
            {
                var installed = await _driverService.GetInstalledDriversAsync(SelectedPlatform);
                InstalledDrivers = new ObservableCollection<Driver>(installed);

                var missing = await _driverService.GetMissingDriversAsync(SelectedPlatform);
                MissingDrivers = new ObservableCollection<Driver>(missing);
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error loading drivers: {ex.Message}", LogLevel.Error, ex);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    /// <summary>
    /// ViewModel for Logs page
    /// </summary>
    public partial class LogsViewModel : ObservableObject
    {
        private readonly ILoggingService _loggingService;

        [ObservableProperty]
        private ObservableCollection<LogEntry> logs = new();

        [ObservableProperty]
        private string filterText = string.Empty;

        [ObservableProperty]
        private bool isLoading = false;

        public LogsViewModel(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        [RelayCommand]
        public async Task LoadLogs()
        {
            IsLoading = true;
            try
            {
                var logs = await _loggingService.GetLogsAsync(500);
                Logs = new ObservableCollection<LogEntry>(logs);
            }
            catch (Exception ex)
            {
                // Log silently to avoid recursion
                System.Diagnostics.Debug.WriteLine($"Error loading logs: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task ExportLogsAsCSV()
        {
            try
            {
                var csvData = await _loggingService.ExportLogsAsCsvAsync();
                SaveExportedLogs(csvData, "TechFixPro_Logs.csv");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error exporting logs: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task ExportLogsAsTXT()
        {
            try
            {
                var txtData = await _loggingService.ExportLogsAsTxtAsync();
                SaveExportedLogs(txtData, "TechFixPro_Logs.txt");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error exporting logs: {ex.Message}");
            }
        }

        private void SaveExportedLogs(byte[] data, string fileName)
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filePath = Path.Combine(desktopPath, fileName);
            File.WriteAllBytes(filePath, data);
        }
    }
}