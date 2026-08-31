namespace TechFix.Pro.Services.ToolsManagement
{
    using TechFix.Pro.Core.Interfaces;
    using TechFix.Pro.Core.Models;
    using TechFix.Pro.Core.Enums;
    using TechFix.Pro.Data.Context;
    using System.Diagnostics;

    /// <summary>
    /// Service for managing external tools
    /// </summary>
    public class ToolsService : IToolsService
    {
        private readonly TechFixDbContext _context;
        private readonly ILoggingService _loggingService;

        public ToolsService(TechFixDbContext context, ILoggingService loggingService)
        {
            _context = context;
            _loggingService = loggingService;
        }

        /// <summary>
        /// Add a new tool
        /// </summary>
        public async Task<bool> AddToolAsync(Tool tool)
        {
            try
            {
                if (!File.Exists(tool.ToolPath))
                {
                    await _loggingService.LogAsync($"Tool file not found: {tool.ToolPath}", LogLevel.Error);
                    return false;
                }

                await _context.Tools.AddAsync(tool);
                await _context.SaveChangesAsync();

                await _loggingService.LogAsync($"Tool added: {tool.Name}", LogLevel.Information);
                return true;
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error adding tool: {ex.Message}", LogLevel.Error, ex);
                return false;
            }
        }

        /// <summary>
        /// Remove a tool
        /// </summary>
        public async Task<bool> RemoveToolAsync(int toolId)
        {
            try
            {
                var tool = await _context.Tools.FindAsync(toolId);
                if (tool != null)
                {
                    _context.Tools.Remove(tool);
                    await _context.SaveChangesAsync();

                    await _loggingService.LogAsync($"Tool removed: {tool.Name}", LogLevel.Information);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error removing tool: {ex.Message}", LogLevel.Error, ex);
                return false;
            }
        }

        /// <summary>
        /// Get tools by platform
        /// </summary>
        public async Task<IEnumerable<Tool>> GetToolsByPlatformAsync(Platform platform)
        {
            try
            {
                return await _context.Tools
                    .Where(t => t.SupportedPlatform == platform)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error getting tools by platform: {ex.Message}", LogLevel.Error, ex);
                return new List<Tool>();
            }
        }

        /// <summary>
        /// Execute a tool
        /// </summary>
        public async Task<bool> ExecuteToolAsync(Tool tool, string? arguments = null)
        {
            try
            {
                if (!File.Exists(tool.ToolPath))
                {
                    await _loggingService.LogAsync($"Tool not found: {tool.ToolPath}", LogLevel.Error);
                    return false;
                }

                var processInfo = new ProcessStartInfo
                {
                    FileName = tool.ToolPath,
                    Arguments = arguments ?? tool.Arguments ?? string.Empty,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {
                    if (process != null)
                    {
                        process.WaitForExit(30000); // 30 second timeout

                        tool.UsageCount++;
                        _context.Tools.Update(tool);
                        await _context.SaveChangesAsync();

                        await _loggingService.LogAsync($"Tool executed: {tool.Name}", LogLevel.Information);
                        return process.ExitCode == 0;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error executing tool: {ex.Message}", LogLevel.Error, ex);
                return false;
            }
        }

        /// <summary>
        /// Get all tools
        /// </summary>
        public async Task<IEnumerable<Tool>> GetAllToolsAsync()
        {
            try
            {
                return await _context.Tools.ToListAsync();
            }
            catch (Exception ex)
            {
                await _loggingService.LogAsync($"Error getting all tools: {ex.Message}", LogLevel.Error, ex);
                return new List<Tool>();
            }
        }
    }
}