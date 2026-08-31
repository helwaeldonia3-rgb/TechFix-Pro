namespace TechFix.Pro.Core.Interfaces
{
    using Models;

    /// <summary>
    /// Interface for device repository operations
    /// </summary>
    public interface IDeviceRepository
    {
        Task<Device?> GetBySerialNumberAsync(string serialNumber);
        Task<IEnumerable<Device>> GetAllAsync();
        Task<Device?> GetByIdAsync(int id);
        Task AddAsync(Device device);
        Task UpdateAsync(Device device);
        Task DeleteAsync(int id);
        Task<Device?> GetLastConnectedAsync();
    }

    /// <summary>
    /// Interface for firmware repository operations
    /// </summary>
    public interface IFirmwareRepository
    {
        Task<Firmware?> GetByIdAsync(int id);
        Task<IEnumerable<Firmware>> GetByManufacturerAsync(string manufacturer);
        Task<IEnumerable<Firmware>> GetByModelAsync(string model);
        Task<Firmware?> GetByHashAsync(string hash);
        Task<IEnumerable<Firmware>> GetAllAsync();
        Task AddAsync(Firmware firmware);
        Task UpdateAsync(Firmware firmware);
        Task DeleteAsync(int id);
        Task<IEnumerable<Firmware>> SearchAsync(string query);
    }

    /// <summary>
    /// Interface for driver repository operations
    /// </summary>
    public interface IDriverRepository
    {
        Task<Driver?> GetByIdAsync(int id);
        Task<IEnumerable<Driver>> GetByPlatformAsync(Enums.Platform platform);
        Task<IEnumerable<Driver>> GetAllAsync();
        Task AddAsync(Driver driver);
        Task UpdateAsync(Driver driver);
        Task DeleteAsync(int id);
        Task<IEnumerable<Driver>> GetInstalledAsync();
    }

    /// <summary>
    /// Interface for tool repository operations
    /// </summary>
    public interface IToolRepository
    {
        Task<Tool?> GetByIdAsync(int id);
        Task<IEnumerable<Tool>> GetByPlatformAsync(Enums.Platform platform);
        Task<IEnumerable<Tool>> GetAllAsync();
        Task AddAsync(Tool tool);
        Task UpdateAsync(Tool tool);
        Task DeleteAsync(int id);
    }

    /// <summary>
    /// Interface for log repository operations
    /// </summary>
    public interface ILogRepository
    {
        Task<LogEntry?> GetByIdAsync(int id);
        Task<IEnumerable<LogEntry>> GetRecentAsync(int count = 100);
        Task<IEnumerable<LogEntry>> GetByDeviceAsync(string serialNumber, int count = 50);
        Task<IEnumerable<LogEntry>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task AddAsync(LogEntry logEntry);
        Task<IEnumerable<LogEntry>> GetAllAsync();
        Task ClearOldEntriesAsync(DateTime beforeDate);
    }
}