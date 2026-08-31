namespace TechFix.Pro.Core.Models
{
    /// <summary>
    /// Represents a connected device
    /// </summary>
    public class Device
    {
        public int Id { get; set; }
        public string? SerialNumber { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? DeviceName { get; set; }
        public string? SoC { get; set; }
        public string? Chipset { get; set; }
        public Enums.CpuArchitecture Architecture { get; set; }
        public Enums.Platform Platform { get; set; }
        public Enums.DeviceBootMode BootMode { get; set; }
        public Enums.ConnectionStatus ConnectionStatus { get; set; }
        public string? UsbVendorId { get; set; }
        public string? UsbProductId { get; set; }
        public string? AndroidVersion { get; set; }
        public string? BuildNumber { get; set; }
        public string? FirmwareVersion { get; set; }
        public int? RAMGb { get; set; }
        public int? StorageGb { get; set; }
        public DateTime FirstDetectedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastDetectedAt { get; set; } = DateTime.UtcNow;
        public bool IsAuthenticated { get; set; }
        public string? IpAddress { get; set; }

        public override string ToString() => $"{Manufacturer} {Model} ({DeviceName})";
    }

    /// <summary>
    /// Represents a firmware file
    /// </summary>
    public class Firmware
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? Version { get; set; }
        public string? BuildNumber { get; set; }
        public string? Chipset { get; set; }
        public long FileSizeBytes { get; set; }
        public string? Sha256Hash { get; set; }
        public string? Md5Hash { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public string? Description { get; set; }
        public bool IsVerified { get; set; }
        public string? Tags { get; set; }
    }

    /// <summary>
    /// Represents a device driver
    /// </summary>
    public class Driver
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Version { get; set; }
        public Enums.Platform Platform { get; set; }
        public Enums.DriverStatus Status { get; set; }
        public string? DriverPath { get; set; }
        public string? Description { get; set; }
        public DateTime InstalledDate { get; set; }
        public bool IsSystemDriver { get; set; }
        public string? CompatibleModels { get; set; }
    }

    /// <summary>
    /// Represents an external tool
    /// </summary>
    public class Tool
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Version { get; set; }
        public string? ToolPath { get; set; }
        public Enums.Platform SupportedPlatform { get; set; }
        public string? Description { get; set; }
        public bool IsInstalled { get; set; }
        public string? Arguments { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public int UsageCount { get; set; }
    }

    /// <summary>
    /// Represents a log entry
    /// </summary>
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public Enums.LogLevel Level { get; set; }
        public string? Message { get; set; }
        public string? Exception { get; set; }
        public string? DeviceSerialNumber { get; set; }
        public string? Operation { get; set; }
        public Enums.OperationResult Result { get; set; }
        public string? ErrorCode { get; set; }
        public string? Details { get; set; }
        public string? UserId { get; set; }
    }

    /// <summary>
    /// Represents application settings
    /// </summary>
    public class AppSetting
    {
        public int Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}