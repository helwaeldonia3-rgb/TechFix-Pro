namespace TechFix.Pro.Services.DeviceDetection
{
    using TechFix.Pro.Core.Interfaces;
    using TechFix.Pro.Core.Models;
    using TechFix.Pro.Core.Enums;
    using TechFix.Pro.Data.Context;
    using System.Management;

    /// <summary>
    /// Service for detecting connected devices via USB
    /// </summary>
    public class DeviceDetectionService : IDeviceDetectionService
    {
        private readonly TechFixDbContext _context;
        private readonly ILoggingService _loggingService;

        public DeviceDetectionService(TechFixDbContext context, ILoggingService loggingService)
        {
            _context = context;
            _loggingService = loggingService;
        }

        /// <summary>
        /// Detect a single connected device
        /// </summary>
        public async Task<Device?> DetectConnectedDeviceAsync()
        {
            try
            {
                var devices = await DetectAllDevicesAsync();
                return devices.FirstOrDefault();
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error detecting device: {ex.Message}", LogLevel.Error, ex);
                return null;
            }
        }

        /// <summary>
        /// Detect all connected devices
        /// </summary>
        public async Task<IEnumerable<Device>> DetectAllDevicesAsync()
        {
            var devices = new List<Device>();

            try
            {
                // Query USB devices
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_USBHub"))
                {
                    foreach (var device in searcher.Get())
                    {
                        var newDevice = await ParseUsbDeviceAsync(device);
                        if (newDevice != null)
                        {
                            devices.Add(newDevice);
                        }
                    }
                }

                await _loggingService.LogAsync($"Detected {devices.Count} device(s)", LogLevel.Information);
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error detecting all devices: {ex.Message}", LogLevel.Error, ex);
            }

            return devices;
        }

        /// <summary>
        /// Get device information by serial number
        /// </summary>
        public async Task<Device?> GetDeviceInfoAsync(string serialNumber)
        {
            try
            {
                var device = await _context.Devices.FirstOrDefaultAsync(d => d.SerialNumber == serialNumber);
                return device;
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error getting device info: {ex.Message}", LogLevel.Error, ex);
                return null;
            }
        }

        /// <summary>
        /// Detect the boot mode of a device
        /// </summary>
        public async Task<DeviceBootMode> DetectBootModeAsync(Device device)
        {
            // This would be implemented with actual USB protocol communication
            // For now, returning a placeholder
            await _loggingService.LogAsync($"Detecting boot mode for {device.Model}", LogLevel.Debug);
            return device.BootMode;
        }

        /// <summary>
        /// Identify the platform based on device characteristics
        /// </summary>
        public async Task<Platform> IdentifyPlatformAsync(Device device)
        {
            var platform = device.Chipset switch
            {
                not null when device.Chipset.Contains("Snapdragon", StringComparison.OrdinalIgnoreCase) => Platform.QualcommSnapdragon,
                not null when device.Chipset.Contains("MediaTek", StringComparison.OrdinalIgnoreCase) => Platform.MediaTek,
                not null when device.Chipset.Contains("Exynos", StringComparison.OrdinalIgnoreCase) => Platform.SamsungExynos,
                not null when device.Chipset.Contains("Bionic", StringComparison.OrdinalIgnoreCase) => Platform.Apple,
                not null when device.Chipset.Contains("Unisoc", StringComparison.OrdinalIgnoreCase) => Platform.Unisoc,
                not null when device.Chipset.Contains("Kirin", StringComparison.OrdinalIgnoreCase) => Platform.HuaweiKirin,
                not null when device.Chipset.Contains("Tensor", StringComparison.OrdinalIgnoreCase) => Platform.GoogleTensor,
                _ => Platform.Unknown
            };

            await _loggingService.LogAsync($"Identified platform: {platform} for {device.Model}", LogLevel.Debug);
            return platform;
        }

        /// <summary>
        /// Check if device is authorized
        /// </summary>
        public async Task<bool> IsDeviceAuthorizedAsync(Device device)
        {
            // Placeholder implementation
            await _loggingService.LogAsync($"Checking authorization for {device.Model}", LogLevel.Debug);
            return device.IsAuthenticated;
        }

        /// <summary>
        /// Parse USB device information
        /// </summary>
        private async Task<Device?> ParseUsbDeviceAsync(ManagementObject usbDevice)
        {
            try
            {
                var device = new Device
                {
                    DeviceName = usbDevice["Name"]?.ToString() ?? "Unknown Device",
                    Manufacturer = usbDevice["Manufacturer"]?.ToString() ?? "Unknown",
                    ConnectionStatus = ConnectionStatus.Connected,
                    FirstDetectedAt = DateTime.UtcNow,
                    LastDetectedAt = DateTime.UtcNow
                };

                // Identify platform if possible
                device.Platform = await IdentifyPlatformAsync(device);

                return device;
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error parsing USB device: {ex.Message}", LogLevel.Warning, ex);
                return null;
            }
        }
    }
}