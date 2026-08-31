namespace TechFix.Pro.Core.Enums
{
    /// <summary>
    /// Enum for different device manufacturers
    /// </summary>
    public enum Manufacturer
    {
        Unknown = 0,
        Qualcomm = 1,
        MediaTek = 2,
        Samsung = 3,
        Apple = 4,
        Unisoc = 5,
        Huawei = 6,
        Google = 7,
        Xiaomi = 8,
        OnePlus = 9,
        Motorola = 10
    }

    /// <summary>
    /// Enum for different SoC (System on Chip) types
    /// </summary>
    public enum SoCType
    {
        Unknown = 0,
        SnapdragonQualcomm = 1,
        Exynos = 2,
        Bionic = 3,
        MediaTekHelio = 4,
        MediaTekDimensity = 5,
        Unisoc = 6,
        KirinHuawei = 7,
        Tensor = 8
    }

    /// <summary>
    /// Enum for device boot modes
    /// </summary>
    public enum DeviceBootMode
    {
        Unknown = 0,
        Normal = 1,
        Fastboot = 2,
        Recovery = 3,
        Download = 4,
        EDL = 5,
        Preloader = 6,
        DFU = 7,
        ADB = 8
    }

    /// <summary>
    /// Enum for device connection status
    /// </summary>
    public enum ConnectionStatus
    {
        Disconnected = 0,
        Connected = 1,
        Authorized = 2,
        Unauthorized = 3,
        Error = 4
    }

    /// <summary>
    /// Enum for supported platforms
    /// </summary>
    public enum Platform
    {
        Unknown = 0,
        QualcommSnapdragon = 1,
        MediaTek = 2,
        SamsungExynos = 3,
        SamsungQualcomm = 4,
        Apple = 5,
        Unisoc = 6,
        HuaweiKirin = 7,
        GoogleTensor = 8
    }

    /// <summary>
    /// Enum for CPU architecture types
    /// </summary>
    public enum CpuArchitecture
    {
        Unknown = 0,
        ARM32 = 1,
        ARM64 = 2,
        x86 = 3,
        x64 = 4
    }

    /// <summary>
    /// Enum for operation result status
    /// </summary>
    public enum OperationResult
    {
        Unknown = 0,
        Success = 1,
        Failed = 2,
        Cancelled = 3,
        Pending = 4,
        Error = 5
    }

    /// <summary>
    /// Enum for driver status
    /// </summary>
    public enum DriverStatus
    {
        Unknown = 0,
        Installed = 1,
        Missing = 2,
        OutOfDate = 3,
        Error = 4
    }

    /// <summary>
    /// Enum for log level severity
    /// </summary>
    public enum LogLevel
    {
        Debug = 0,
        Information = 1,
        Warning = 2,
        Error = 3,
        Critical = 4
    }
}