# TechFix Pro - Professional Smartphone Repair Tool

## Overview
TechFix Pro is a professional-grade Windows desktop application designed for smartphone maintenance technicians. It provides comprehensive tools and utilities for managing devices across multiple platforms including Qualcomm, MediaTek, Samsung, Apple, and Unisoc.

## Features

### Core Functionality
- **Device Detection**: Automatic detection and identification of connected devices
- **Platform Support**: 
  - Qualcomm Snapdragon
  - MediaTek (Helio, Dimensity)
  - Samsung Exynos & Qualcomm
  - Apple iOS
  - Unisoc SC/SG Series

### Device Management
- Device information display
- Boot mode detection
- Connection status monitoring
- Device authorization verification

### Firmware Management
- Firmware library management
- File integrity verification (SHA256/MD5)
- Firmware metadata tracking
- Search and filtering capabilities

### Driver Management
- Platform-specific driver management
- Driver status tracking
- Installation/uninstallation support
- Compatibility checking

### Tool Management
- External tool integration
- Tool execution with parameter support
- Usage tracking
- Platform-specific tool organization

### Logging & Reporting
- Comprehensive application logging
- Device operation history
- Log export (CSV/TXT formats)
- Date-range filtering
- Automatic old log cleanup

## Project Structure

```
TechFix-Pro/
├── src/
│   ├── TechFix.Pro/                    # Main WinUI3 Application
│   │   ├── App.xaml(.cs)              # Application entry point
│   │   ├── MainWindow.xaml(.cs)       # Main application window
│   │   ├── ViewModels/                # MVVM ViewModels
│   │   └── Views/                     # XAML Pages
│   │       └── Platforms/             # Platform-specific pages
│   ├── TechFix.Pro.Core/              # Core business logic
│   │   ├── Enums/                    # Enumerations
│   │   ├── Models/                   # Data models
│   │   ├── Interfaces/               # Service interfaces
│   │   └── Constants/                # Application constants
│   ├── TechFix.Pro.Services/          # Business services
│   │   ├── DeviceDetection/          # Device detection service
│   │   ├── FirmwareManagement/       # Firmware service
│   │   ├── DriverManagement/         # Driver service
│   │   ├── ToolsManagement/          # Tools service
│   │   └── Logging/                  # Logging service
│   └── TechFix.Pro.Data/              # Data access layer
│       ├── Context/                  # Entity Framework DbContext
│       ├── Repositories/             # Repository implementations
│       └── Migrations/               # Database migrations
└── .gitignore
```

## Technology Stack

### Framework & UI
- **Windows App SDK 1.4**: WinUI 3 for modern Windows desktop UI
- **.NET 8.0**: Latest .NET runtime

### Architecture
- **MVVM Pattern**: Community Toolkit MVVM
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection
- **Entity Framework Core 8.0**: ORM with SQLite

### Data
- **SQLite**: Local database storage
- **Entity Framework Core**: Database abstraction

### Logging
- **Serilog 3.1**: Structured logging with file and console sinks

## Getting Started

### Prerequisites
- Windows 10 (Build 19041) or later
- .NET 8.0 SDK or Runtime
- Visual Studio 2022 (recommended) or other C# IDE

### Installation
1. Clone the repository
2. Open the solution in Visual Studio
3. Restore NuGet packages
4. Build the solution
5. Run the application

## Configuration

Application settings are configured in `appsettings.json`:
- Log retention period (default: 90 days)
- Maximum log entries (default: 100,000)
- Auto-detection settings
- Timeout configurations

## Database

The application uses SQLite for data persistence. The database file is stored in:
```
%APPDATA%\TechFix Pro\TechFixPro.db
```

Entity Framework migrations are used for schema management.

## Services

### IDeviceDetectionService
- Detects connected USB devices
- Identifies device manufacturers and models
- Determines device boot modes and platforms
- Verifies device authorization status

### IFirmwareService
- Manages firmware file library
- Calculates file hashes for verification
- Tracks firmware metadata
- Supports firmware search and filtering

### IDriverService
- Manages device drivers per platform
- Tracks driver installation status
- Supports driver installation/uninstallation
- Monitors driver status changes

### IToolsService
- Manages external tools and utilities
- Executes tools with custom arguments
- Tracks tool usage
- Organizes tools by platform

### ILoggingService
- Logs application events and operations
- Exports logs in multiple formats (CSV, TXT)
- Maintains operation history per device
- Automatically cleans up old logs

## Development

### Building
```bash
dotnet build
```

### Running
```bash
dotnet run --project src/TechFix.Pro
```

### Testing
Unit tests and integration tests are recommended for service layer implementations.

## License
Proprietary - All rights reserved

## Contributing
Contributions welcome. Please follow the existing code structure and patterns.

## Support
For issues, feature requests, or questions, please contact the development team.

---

**Version**: 1.0.0  
**Last Updated**: September 2026
