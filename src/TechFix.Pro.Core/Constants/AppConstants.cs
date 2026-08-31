namespace TechFix.Pro.Core.Constants
{
    /// <summary>
    /// Application constants
    /// </summary>
    public static class AppConstants
    {
        public const string AppName = "TechFix Pro";
        public const string AppVersion = "1.0.0";
        public const string DatabaseName = "TechFixPro.db";
        public const int MaxLogEntries = 100000;
        public const int LogRetentionDays = 90;
    }

    /// <summary>
    /// USB Vendor and Product IDs for device detection
    /// </summary>
    public static class UsbIds
    {
        // Qualcomm
        public const string QualcommVendorId = "05C6";
        public const string QualcommEDLProductId = "9008";
        public const string QualcommFastbootProductId = "D00D";

        // MediaTek
        public const string MediaTekVendorId = "0E8D";
        public const string MediaTekMetaProductId = "0000";

        // Samsung
        public const string SamsungVendorId = "04E8";

        // Apple
        public const string AppleVendorId = "05AC";

        // Google
        public const string GoogleVendorId = "18D1";
    }

    /// <summary>
    /// Boot mode identifiers
    /// </summary>
    public static class BootModeIdentifiers
    {
        public const string Fastboot = "fastboot";
        public const string Recovery = "recovery";
        public const string Download = "download";
        public const string EDL = "edl";
        public const string Preloader = "preloader";
        public const string DFU = "dfu";
    }
}