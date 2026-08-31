namespace TechFix.Pro.Data.Context
{
    using Microsoft.EntityFrameworkCore;
    using TechFix.Pro.Core.Models;
    using System.IO;

    /// <summary>
    /// Entity Framework Core DbContext for TechFix Pro database
    /// </summary>
    public class TechFixDbContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<Firmware> Firmwares { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<LogEntry> Logs { get; set; }
        public DbSet<AppSetting> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TechFix Pro",
                "TechFixPro.db"
            );

            Directory.CreateDirectory(Path.GetDirectoryName(dbPath) ?? string.Empty);

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Device configuration
            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SerialNumber).HasMaxLength(255);
                entity.Property(e => e.Manufacturer).HasMaxLength(100);
                entity.Property(e => e.Model).HasMaxLength(100);
                entity.Property(e => e.DeviceName).HasMaxLength(255);
                entity.Property(e => e.SoC).HasMaxLength(100);
                entity.Property(e => e.Chipset).HasMaxLength(100);
                entity.Property(e => e.UsbVendorId).HasMaxLength(10);
                entity.Property(e => e.UsbProductId).HasMaxLength(10);
                entity.Property(e => e.AndroidVersion).HasMaxLength(50);
                entity.Property(e => e.BuildNumber).HasMaxLength(100);
                entity.Property(e => e.FirmwareVersion).HasMaxLength(100);
                entity.Property(e => e.IpAddress).HasMaxLength(50);
                entity.HasIndex(e => e.SerialNumber).IsUnique();
            });

            // Firmware configuration
            modelBuilder.Entity<Firmware>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).HasMaxLength(255);
                entity.Property(e => e.FilePath).HasMaxLength(1024);
                entity.Property(e => e.Manufacturer).HasMaxLength(100);
                entity.Property(e => e.Model).HasMaxLength(100);
                entity.Property(e => e.Version).HasMaxLength(100);
                entity.Property(e => e.BuildNumber).HasMaxLength(100);
                entity.Property(e => e.Chipset).HasMaxLength(100);
                entity.Property(e => e.Sha256Hash).HasMaxLength(64);
                entity.Property(e => e.Md5Hash).HasMaxLength(32);
                entity.Property(e => e.Description).HasMaxLength(1024);
                entity.Property(e => e.Tags).HasMaxLength(500);
                entity.HasIndex(e => new { e.Manufacturer, e.Model });
            });

            // Driver configuration
            modelBuilder.Entity<Driver>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Version).HasMaxLength(50);
                entity.Property(e => e.DriverPath).HasMaxLength(1024);
                entity.Property(e => e.Description).HasMaxLength(1024);
                entity.Property(e => e.CompatibleModels).HasMaxLength(500);
            });

            // Tool configuration
            modelBuilder.Entity<Tool>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Version).HasMaxLength(50);
                entity.Property(e => e.ToolPath).HasMaxLength(1024);
                entity.Property(e => e.Description).HasMaxLength(1024);
                entity.Property(e => e.Arguments).HasMaxLength(1024);
            });

            // LogEntry configuration
            modelBuilder.Entity<LogEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Message).HasMaxLength(1024);
                entity.Property(e => e.Exception).HasMaxLength(4096);
                entity.Property(e => e.DeviceSerialNumber).HasMaxLength(255);
                entity.Property(e => e.Operation).HasMaxLength(255);
                entity.Property(e => e.ErrorCode).HasMaxLength(50);
                entity.Property(e => e.Details).HasMaxLength(2048);
                entity.Property(e => e.UserId).HasMaxLength(255);
                entity.HasIndex(e => e.Timestamp);
                entity.HasIndex(e => e.DeviceSerialNumber);
            });

            // AppSetting configuration
            modelBuilder.Entity<AppSetting>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Key).HasMaxLength(100);
                entity.Property(e => e.Value).HasMaxLength(2048);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasIndex(e => e.Key).IsUnique();
            });
        }
    }
}