namespace TechFix.Pro.Services.Logging
{
    using TechFix.Pro.Core.Interfaces;
    using TechFix.Pro.Core.Models;
    using TechFix.Pro.Core.Enums;
    using TechFix.Pro.Data.Context;
    using System.Text;
    using System.Text.Encodings.Web;
    using Serilog;

    /// <summary>
    /// Service for application logging
    /// </summary>
    public class LoggingService : ILoggingService
    {
        private readonly TechFixDbContext _context;
        private readonly ILogger _logger;

        public LoggingService(TechFixDbContext context)
        {
            _context = context;
            _logger = Log.ForContext<LoggingService>();
        }

        /// <summary>
        /// Log a message
        /// </summary>
        public async Task LogAsync(string message, LogLevel level = LogLevel.Information, Exception? exception = null)
        {
            try
            {
                var logEntry = new LogEntry
                {
                    Message = message,
                    Level = level,
                    Exception = exception?.ToString(),
                    Timestamp = DateTime.UtcNow
                };

                await _context.Logs.AddAsync(logEntry);
                await _context.SaveChangesAsync();

                // Also log to Serilog
                switch (level)
                {
                    case LogLevel.Debug:
                        _logger.Debug(exception, message);
                        break;
                    case LogLevel.Information:
                        _logger.Information(exception, message);
                        break;
                    case LogLevel.Warning:
                        _logger.Warning(exception, message);
                        break;
                    case LogLevel.Error:
                        _logger.Error(exception, message);
                        break;
                    case LogLevel.Critical:
                        _logger.Fatal(exception, message);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error logging message");
            }
        }

        /// <summary>
        /// Log an operation result
        /// </summary>
        public async Task LogOperationAsync(string operation, Device? device, OperationResult result, string? errorCode = null, string? details = null)
        {
            try
            {
                var logEntry = new LogEntry
                {
                    Timestamp = DateTime.UtcNow,
                    Level = result == OperationResult.Success ? LogLevel.Information : LogLevel.Warning,
                    Message = $"Operation: {operation}",
                    Operation = operation,
                    DeviceSerialNumber = device?.SerialNumber,
                    Result = result,
                    ErrorCode = errorCode,
                    Details = details
                };

                await _context.Logs.AddAsync(logEntry);
                await _context.SaveChangesAsync();

                _logger.Information($"Operation {operation} completed with result: {result}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error logging operation");
            }
        }

        /// <summary>
        /// Get recent logs
        /// </summary>
        public async Task<IEnumerable<LogEntry>> GetLogsAsync(int count = 100)
        {
            try
            {
                return await _context.Logs
                    .OrderByDescending(l => l.Timestamp)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting logs");
                return new List<LogEntry>();
            }
        }

        /// <summary>
        /// Get logs by device
        /// </summary>
        public async Task<IEnumerable<LogEntry>> GetLogsByDeviceAsync(string serialNumber)
        {
            try
            {
                return await _context.Logs
                    .Where(l => l.DeviceSerialNumber == serialNumber)
                    .OrderByDescending(l => l.Timestamp)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting logs by device");
                return new List<LogEntry>();
            }
        }

        /// <summary>
        /// Export logs as CSV
        /// </summary>
        public async Task<byte[]> ExportLogsAsCsvAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var logs = await GetFilteredLogsAsync(startDate, endDate);

                var csv = new StringBuilder();
                csv.AppendLine("Timestamp,Level,Message,Device,Operation,Result,ErrorCode");

                foreach (var log in logs)
                {
                    csv.AppendLine($"\"{log.Timestamp:yyyy-MM-dd HH:mm:ss}\",\"{log.Level}\",\"{EscapeCsv(log.Message)}\",\"{log.DeviceSerialNumber}\",\"{log.Operation}\",\"{log.Result}\",\"{log.ErrorCode}\"");
                }

                return Encoding.UTF8.GetBytes(csv.ToString());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error exporting logs as CSV");
                return Array.Empty<byte>();
            }
        }

        /// <summary>
        /// Export logs as TXT
        /// </summary>
        public async Task<byte[]> ExportLogsAsTxtAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var logs = await GetFilteredLogsAsync(startDate, endDate);

                var txt = new StringBuilder();
                txt.AppendLine("=== TechFix Pro Logs ===");
                txt.AppendLine($"Exported: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                txt.AppendLine("\n");

                foreach (var log in logs)
                {
                    txt.AppendLine($"[{log.Timestamp:yyyy-MM-dd HH:mm:ss}] [{log.Level}] {log.Message}");
                    if (!string.IsNullOrEmpty(log.DeviceSerialNumber))
                        txt.AppendLine($"  Device: {log.DeviceSerialNumber}");
                    if (!string.IsNullOrEmpty(log.Operation))
                        txt.AppendLine($"  Operation: {log.Operation}");
                    if (!string.IsNullOrEmpty(log.Details))
                        txt.AppendLine($"  Details: {log.Details}");
                    txt.AppendLine();
                }

                return Encoding.UTF8.GetBytes(txt.ToString());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error exporting logs as TXT");
                return Array.Empty<byte>();
            }
        }

        /// <summary>
        /// Clear old log entries
        /// </summary>
        public async Task ClearOldLogsAsync(int daysOld = 30)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow.AddDays(-daysOld);
                var oldLogs = _context.Logs.Where(l => l.Timestamp < cutoffDate);
                _context.Logs.RemoveRange(oldLogs);
                await _context.SaveChangesAsync();

                _logger.Information($"Cleared logs older than {daysOld} days");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error clearing old logs");
            }
        }

        /// <summary>
        /// Get filtered logs by date range
        /// </summary>
        private async Task<IEnumerable<LogEntry>> GetFilteredLogsAsync(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Logs.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(l => l.Timestamp >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(l => l.Timestamp <= endDate.Value);

            return await query.OrderByDescending(l => l.Timestamp).ToListAsync();
        }

        /// <summary>
        /// Escape CSV special characters
        /// </summary>
        private static string EscapeCsv(string? text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text.Replace("\"", "\"\"");
        }
    }
}