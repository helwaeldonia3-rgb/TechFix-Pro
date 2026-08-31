namespace TechFix.Pro.Services.FirmwareManagement
{
    using TechFix.Pro.Core.Interfaces;
    using TechFix.Pro.Core.Models;
    using TechFix.Pro.Core.Enums;
    using TechFix.Pro.Data.Context;
    using System.Security.Cryptography;

    /// <summary>
    /// Service for managing firmware files
    /// </summary>
    public class FirmwareService : IFirmwareService
    {
        private readonly TechFixDbContext _context;
        private readonly ILogRepository _logRepository;
        private readonly ILoggingService _loggingService;

        public FirmwareService(TechFixDbContext context, ILogRepository logRepository, ILoggingService loggingService)
        {
            _context = context;
            _logRepository = logRepository;
            _loggingService = loggingService;
        }

        /// <summary>
        /// Add a firmware file to the library
        /// </summary>
        public async Task<bool> AddFirmwareAsync(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    await _loggingService.LogAsync($"Firmware file not found: {filePath}", LogLevel.Error);
                    return false;
                }

                var fileInfo = new FileInfo(filePath);
                var firmware = new Firmware
                {
                    FileName = fileInfo.Name,
                    FilePath = filePath,
                    FileSizeBytes = fileInfo.Length,
                    AddedDate = DateTime.UtcNow,
                    Sha256Hash = await CalculateFileHashAsync(filePath),
                    IsVerified = false
                };

                await _context.Firmwares.AddAsync(firmware);
                await _context.SaveChangesAsync();

                await _loggingService.LogAsync($"Firmware added: {firmware.FileName}", LogLevel.Information);
                return true;
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error adding firmware: {ex.Message}", LogLevel.Error, ex);
                return false;
            }
        }

        /// <summary>
        /// Get firmware by ID
        /// </summary>
        public async Task<Firmware?> GetFirmwareAsync(int id)
        {
            try
            {
                return await _context.Firmwares.FindAsync(id);
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error getting firmware: {ex.Message}", LogLevel.Error, ex);
                return null;
            }
        }

        /// <summary>
        /// Search firmware by manufacturer and model
        /// </summary>
        public async Task<IEnumerable<Firmware>> SearchFirmwareAsync(string manufacturer, string model)
        {
            try
            {
                return await _context.Firmwares
                    .Where(f => f.Manufacturer == manufacturer && f.Model == model)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error searching firmware: {ex.Message}", LogLevel.Error, ex);
                return new List<Firmware>();
            }
        }

        /// <summary>
        /// Calculate file hash (SHA256)
        /// </summary>
        public async Task<string?> CalculateFileHashAsync(string filePath)
        {
            try
            {
                using (var sha256 = SHA256.Create())
                {
                    using (var stream = File.OpenRead(filePath))
                    {
                        var hash = await Task.Run(() => sha256.ComputeHash(stream));
                        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error calculating hash: {ex.Message}", LogLevel.Error, ex);
                return null;
            }
        }

        /// <summary>
        /// Verify firmware integrity
        /// </summary>
        public async Task<bool> VerifyFirmwareIntegrityAsync(Firmware firmware)
        {
            try
            {
                if (!File.Exists(firmware.FilePath))
                {
                    await _loggingService.LogAsync($"Firmware file not found for verification: {firmware.FilePath}", LogLevel.Warning);
                    return false;
                }

                var currentHash = await CalculateFileHashAsync(firmware.FilePath);
                var isValid = currentHash == firmware.Sha256Hash;

                if (isValid)
                {
                    firmware.IsVerified = true;
                    _context.Firmwares.Update(firmware);
                    await _context.SaveChangesAsync();
                }

                await _loggingService.LogAsync($"Firmware verification: {(isValid ? "Success" : "Failed")} for {firmware.FileName}", LogLevel.Information);
                return isValid;
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error verifying firmware: {ex.Message}", LogLevel.Error, ex);
                return false;
            }
        }

        /// <summary>
        /// Delete firmware
        /// </summary>
        public async Task<bool> DeleteFirmwareAsync(int id)
        {
            try
            {
                var firmware = await GetFirmwareAsync(id);
                if (firmware != null)
                {
                    _context.Firmwares.Remove(firmware);
                    await _context.SaveChangesAsync();
                    await _loggingService.LogAsync($"Firmware deleted: {firmware.FileName}", LogLevel.Information);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error deleting firmware: {ex.Message}", LogLevel.Error, ex);
                return false;
            }
        }

        /// <summary>
        /// Get all firmwares
        /// </summary>
        public async Task<IEnumerable<Firmware>> GetAllFirmwaresAsync()
        {
            try
            {
                return await _context.Firmwares.ToListAsync();
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error getting all firmwares: {ex.Message}", LogLevel.Error, ex);
                return new List<Firmware>();
            }
        }
    }
}