namespace TechFix.Pro.Services.DriverManagement
{
    using TechFix.Pro.Core.Interfaces;
    using TechFix.Pro.Core.Models;
    using TechFix.Pro.Core.Enums;
    using TechFix.Pro.Data.Context;

    /// <summary>
    /// Service for managing device drivers
    /// </summary>
    public class DriverService : IDriverService
    {
        private readonly TechFixDbContext _context;
        private readonly ILoggingService _loggingService;

        public DriverService(TechFixDbContext context, ILoggingService loggingService)
        {
            _context = context;
            _loggingService = loggingService;
        }

        /// <summary>
        /// Get installed drivers for a platform
        /// </summary>
        public async Task<IEnumerable<Driver>> GetInstalledDriversAsync(Platform platform)
        {
            try
            {
                var drivers = await _context.Drivers
                    .Where(d => d.Platform == platform && d.Status == DriverStatus.Installed)
                    .ToListAsync();

                await _loggingService.LogAsync($"Retrieved {drivers.Count} installed drivers for {platform}", LogLevel.Debug);
                return drivers;
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error getting installed drivers: {ex.Message}", LogLevel.Error, ex);
                return new List<Driver>();
            }
        }

        /// <summary>
        /// Get missing drivers for a platform
        /// </summary>
        public async Task<IEnumerable<Driver>> GetMissingDriversAsync(Platform platform)
        {
            try
            {
                var drivers = await _context.Drivers
                    .Where(d => d.Platform == platform && (d.Status == DriverStatus.Missing || d.Status == DriverStatus.OutOfDate))
                    .ToListAsync();

                await _loggingService.LogAsync($"Retrieved {drivers.Count} missing drivers for {platform}", LogLevel.Debug);
                return drivers;
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error getting missing drivers: {ex.Message}", LogLevel.Error, ex);
                return new List<Driver>();
            }
        }

        /// <summary>
        /// Install a driver
        /// </summary>
        public async Task<bool> InstallDriverAsync(Driver driver)
        {
            try
            {
                if (!File.Exists(driver.DriverPath))
                {
                    await _loggingService.LogAsync($"Driver file not found: {driver.DriverPath}", LogLevel.Error);
                    return false;
                }

                driver.Status = DriverStatus.Installed;
                driver.InstalledDate = DateTime.UtcNow;

                _context.Drivers.Update(driver);
                await _context.SaveChangesAsync();

                await _loggingService.LogAsync($"Driver installed: {driver.Name}", LogLevel.Information);
                return true;
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error installing driver: {ex.Message}", LogLevel.Error, ex);
                return false;
            }
        }

        /// <summary>
        /// Uninstall a driver
        /// </summary>
        public async Task<bool> UninstallDriverAsync(int driverId)
        {
            try
            {
                var driver = await _context.Drivers.FindAsync(driverId);
                if (driver != null)
                {
                    driver.Status = DriverStatus.Missing;
                    _context.Drivers.Update(driver);
                    await _context.SaveChangesAsync();

                    await _loggingService.LogAsync($"Driver uninstalled: {driver.Name}", LogLevel.Information);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error uninstalling driver: {ex.Message}", LogLevel.Error, ex);
                return false;
            }
        }

        /// <summary>
        /// Check driver status
        /// </summary>
        public async Task<DriverStatus> CheckDriverStatusAsync(Driver driver)
        {
            try
            {
                if (!File.Exists(driver.DriverPath))
                {
                    driver.Status = DriverStatus.Missing;
                }

                // Additional status checks can be implemented here
                await _loggingService.LogAsync($"Driver status check: {driver.Name} - {driver.Status}", LogLevel.Debug);
                return driver.Status;
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error checking driver status: {ex.Message}", LogLevel.Error, ex);
                return DriverStatus.Error;
            }
        }
    }
}