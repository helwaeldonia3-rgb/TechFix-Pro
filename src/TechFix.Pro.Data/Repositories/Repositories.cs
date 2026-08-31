namespace TechFix.Pro.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using TechFix.Pro.Core.Interfaces;
    using TechFix.Pro.Core.Models;
    using TechFix.Pro.Data.Context;

    /// <summary>
    /// Repository implementation for Device entity
    /// </summary>
    public class DeviceRepository : IDeviceRepository
    {
        private readonly TechFixDbContext _context;

        public DeviceRepository(TechFixDbContext context)
        {
            _context = context;
        }

        public async Task<Device?> GetBySerialNumberAsync(string serialNumber)
        {
            return await _context.Devices.FirstOrDefaultAsync(d => d.SerialNumber == serialNumber);
        }

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            return await _context.Devices.ToListAsync();
        }

        public async Task<Device?> GetByIdAsync(int id)
        {
            return await _context.Devices.FindAsync(id);
        }

        public async Task AddAsync(Device device)
        {
            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Device device)
        {
            _context.Devices.Update(device);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var device = await GetByIdAsync(id);
            if (device != null)
            {
                _context.Devices.Remove(device);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Device?> GetLastConnectedAsync()
        {
            return await _context.Devices.OrderByDescending(d => d.LastDetectedAt).FirstOrDefaultAsync();
        }
    }

    /// <summary>
    /// Repository implementation for Firmware entity
    /// </summary>
    public class FirmwareRepository : IFirmwareRepository
    {
        private readonly TechFixDbContext _context;

        public FirmwareRepository(TechFixDbContext context)
        {
            _context = context;
        }

        public async Task<Firmware?> GetByIdAsync(int id)
        {
            return await _context.Firmwares.FindAsync(id);
        }

        public async Task<IEnumerable<Firmware>> GetByManufacturerAsync(string manufacturer)
        {
            return await _context.Firmwares.Where(f => f.Manufacturer == manufacturer).ToListAsync();
        }

        public async Task<IEnumerable<Firmware>> GetByModelAsync(string model)
        {
            return await _context.Firmwares.Where(f => f.Model == model).ToListAsync();
        }

        public async Task<Firmware?> GetByHashAsync(string hash)
        {
            return await _context.Firmwares.FirstOrDefaultAsync(f => f.Sha256Hash == hash || f.Md5Hash == hash);
        }

        public async Task<IEnumerable<Firmware>> GetAllAsync()
        {
            return await _context.Firmwares.ToListAsync();
        }

        public async Task AddAsync(Firmware firmware)
        {
            await _context.Firmwares.AddAsync(firmware);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Firmware firmware)
        {
            _context.Firmwares.Update(firmware);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var firmware = await GetByIdAsync(id);
            if (firmware != null)
            {
                _context.Firmwares.Remove(firmware);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Firmware>> SearchAsync(string query)
        {
            var lower = query.ToLower();
            return await _context.Firmwares.Where(f =>
                f.FileName!.Contains(lower, StringComparison.OrdinalIgnoreCase) ||
                f.Model!.Contains(lower, StringComparison.OrdinalIgnoreCase) ||
                f.Version!.Contains(lower, StringComparison.OrdinalIgnoreCase) ||
                f.Manufacturer!.Contains(lower, StringComparison.OrdinalIgnoreCase)
            ).ToListAsync();
        }
    }

    /// <summary>
    /// Repository implementation for Driver entity
    /// </summary>
    public class DriverRepository : IDriverRepository
    {
        private readonly TechFixDbContext _context;

        public DriverRepository(TechFixDbContext context)
        {
            _context = context;
        }

        public async Task<Driver?> GetByIdAsync(int id)
        {
            return await _context.Drivers.FindAsync(id);
        }

        public async Task<IEnumerable<Driver>> GetByPlatformAsync(Core.Enums.Platform platform)
        {
            return await _context.Drivers.Where(d => d.Platform == platform).ToListAsync();
        }

        public async Task<IEnumerable<Driver>> GetAllAsync()
        {
            return await _context.Drivers.ToListAsync();
        }

        public async Task AddAsync(Driver driver)
        {
            await _context.Drivers.AddAsync(driver);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Driver driver)
        {
            _context.Drivers.Update(driver);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var driver = await GetByIdAsync(id);
            if (driver != null)
            {
                _context.Drivers.Remove(driver);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Driver>> GetInstalledAsync()
        {
            return await _context.Drivers.Where(d => d.Status == Core.Enums.DriverStatus.Installed).ToListAsync();
        }
    }

    /// <summary>
    /// Repository implementation for Tool entity
    /// </summary>
    public class ToolRepository : IToolRepository
    {
        private readonly TechFixDbContext _context;

        public ToolRepository(TechFixDbContext context)
        {
            _context = context;
        }

        public async Task<Tool?> GetByIdAsync(int id)
        {
            return await _context.Tools.FindAsync(id);
        }

        public async Task<IEnumerable<Tool>> GetByPlatformAsync(Core.Enums.Platform platform)
        {
            return await _context.Tools.Where(t => t.SupportedPlatform == platform).ToListAsync();
        }

        public async Task<IEnumerable<Tool>> GetAllAsync()
        {
            return await _context.Tools.ToListAsync();
        }

        public async Task AddAsync(Tool tool)
        {
            await _context.Tools.AddAsync(tool);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tool tool)
        {
            _context.Tools.Update(tool);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tool = await GetByIdAsync(id);
            if (tool != null)
            {
                _context.Tools.Remove(tool);
                await _context.SaveChangesAsync();
            }
        }
    }

    /// <summary>
    /// Repository implementation for LogEntry entity
    /// </summary>
    public class LogRepository : ILogRepository
    {
        private readonly TechFixDbContext _context;

        public LogRepository(TechFixDbContext context)
        {
            _context = context;
        }

        public async Task<LogEntry?> GetByIdAsync(int id)
        {
            return await _context.Logs.FindAsync(id);
        }

        public async Task<IEnumerable<LogEntry>> GetRecentAsync(int count = 100)
        {
            return await _context.Logs.OrderByDescending(l => l.Timestamp).Take(count).ToListAsync();
        }

        public async Task<IEnumerable<LogEntry>> GetByDeviceAsync(string serialNumber, int count = 50)
        {
            return await _context.Logs
                .Where(l => l.DeviceSerialNumber == serialNumber)
                .OrderByDescending(l => l.Timestamp)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<LogEntry>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Logs
                .Where(l => l.Timestamp >= startDate && l.Timestamp <= endDate)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }

        public async Task AddAsync(LogEntry logEntry)
        {
            await _context.Logs.AddAsync(logEntry);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LogEntry>> GetAllAsync()
        {
            return await _context.Logs.OrderByDescending(l => l.Timestamp).ToListAsync();
        }

        public async Task ClearOldEntriesAsync(DateTime beforeDate)
        {
            var oldEntries = _context.Logs.Where(l => l.Timestamp < beforeDate);
            _context.Logs.RemoveRange(oldEntries);
            await _context.SaveChangesAsync();
        }
    }
}