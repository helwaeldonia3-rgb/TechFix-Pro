namespace TechFix.Pro.Core.Interfaces
{
    using Models;
    using Enums;

    /// <summary>
    /// Interface for device detection service
    /// </summary>
    public interface IDeviceDetectionService
    {
        Task<Device?> DetectConnectedDeviceAsync();
        Task<IEnumerable<Device>> DetectAllDevicesAsync();
        Task<Device?> GetDeviceInfoAsync(string serialNumber);
        Task<DeviceBootMode> DetectBootModeAsync(Device device);
        Task<Platform> IdentifyPlatformAsync(Device device);
        Task<bool> IsDeviceAuthorizedAsync(Device device);
    }

    /// <summary>
    /// Interface for firmware service
    /// </summary>
    public interface IFirmwareService
    {
        Task<bool> AddFirmwareAsync(string filePath);
        Task<Firmware?> GetFirmwareAsync(int id);
        Task<IEnumerable<Firmware>> SearchFirmwareAsync(string manufacturer, string model);
        Task<string?> CalculateFileHashAsync(string filePath);
        Task<bool> VerifyFirmwareIntegrityAsync(Firmware firmware);
        Task<bool> DeleteFirmwareAsync(int id);
        Task<IEnumerable<Firmware>> GetAllFirmwaresAsync();
    }

    /// <summary>
    /// Interface for driver service
    /// </summary>
    public interface IDriverService
    {
        Task<IEnumerable<Driver>> GetInstalledDriversAsync(Platform platform);
        Task<IEnumerable<Driver>> GetMissingDriversAsync(Platform platform);
        Task<bool> InstallDriverAsync(Driver driver);
        Task<bool> UninstallDriverAsync(int driverId);
        Task<DriverStatus> CheckDriverStatusAsync(Driver driver);
    }

    /// <summary>
    /// Interface for tools service
    /// </summary>
    public interface IToolsService
    {
        Task<bool> AddToolAsync(Tool tool);
        Task<bool> RemoveToolAsync(int toolId);
        Task<IEnumerable<Tool>> GetToolsByPlatformAsync(Platform platform);
        Task<bool> ExecuteToolAsync(Tool tool, string? arguments = null);
        Task<IEnumerable<Tool>> GetAllToolsAsync();
    }

    /// <summary>
    /// Interface for logging service
    /// </summary>
    public interface ILoggingService
    {
        Task LogAsync(string message, LogLevel level = LogLevel.Information, Exception? exception = null);
        Task LogOperationAsync(string operation, Device? device, OperationResult result, string? errorCode = null, string? details = null);
        Task<IEnumerable<LogEntry>> GetLogsAsync(int count = 100);
        Task<IEnumerable<LogEntry>> GetLogsByDeviceAsync(string serialNumber);
        Task<byte[]> ExportLogsAsCsvAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<byte[]> ExportLogsAsTxtAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task ClearOldLogsAsync(int daysOld = 30);
    }
}