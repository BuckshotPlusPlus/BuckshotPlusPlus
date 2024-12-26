using LibGit2Sharp;
using BuckshotPlusPlus.Models;
using Serilog;
using System.IO;
using System.Threading.Tasks;
using System;

namespace BuckshotPlusPlus.Services
{
    public static class GitService
    {
        private static readonly ILogger _logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/git.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        public static async Task UpdateRepository(Tenant tenant, string siteDir)
        {
            try
            {
                // Try to safely clean up the directory first
                if (Directory.Exists(siteDir))
                {
                    await SafeDeleteDirectory(siteDir);
                }

                // Create the directory
                Directory.CreateDirectory(siteDir);

                _logger.Information("Cloning repository for {Domain} into {SiteDir}", tenant.Domain, siteDir);

                var options = new CloneOptions
                {
                    BranchName = tenant.Branch,
                    CredentialsProvider = (_url, _user, _cred) => GetCredentials(tenant.Auth)
                };

                try
                {
                    Repository.Clone(tenant.RepoUrl, siteDir, options);
                    _logger.Information("Successfully cloned repository for {Domain}", tenant.Domain);

                    // Ensure write permissions on Linux/Unix
                    if (!OperatingSystem.IsWindows())
                    {
                        await SetUnixPermissions(siteDir);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to clone repository for {Domain}", tenant.Domain);
                    throw new Exception($"Git clone failed: {ex.Message}", ex);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Git operation failed for {Domain}", tenant.Domain);
                throw new Exception($"Git operation failed: {ex.Message}", ex);
            }
        }

        private static async Task SafeDeleteDirectory(string path)
        {
            try
            {
                _logger.Debug("Attempting to delete directory: {Path}", path);

                // Wait for up to 5 seconds for any file operations to complete
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        if (Directory.Exists(path))
                        {
                            // Reset file attributes for all files
                            foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
                            {
                                if (OperatingSystem.IsWindows())
                                {
                                    File.SetAttributes(file, FileAttributes.Normal);
                                }
                            }

                            // Delete directory
                            Directory.Delete(path, true);
                            _logger.Debug("Successfully deleted directory: {Path}", path);
                            return;
                        }
                        else
                        {
                            return; // Directory doesn't exist, no need to delete
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        _logger.Warning("Access denied, retrying after delay... Attempt {Attempt}/5", i + 1);
                        await Task.Delay(1000); // Wait 1 second before retry
                    }
                    catch (IOException)
                    {
                        _logger.Warning("IO Exception, retrying after delay... Attempt {Attempt}/5", i + 1);
                        await Task.Delay(1000);
                    }
                }

                throw new Exception($"Failed to delete directory after 5 attempts: {path}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to safely delete directory: {Path}", path);
                throw;
            }
        }

        private static async Task SetUnixPermissions(string path)
        {
            try
            {
                // Use chmod through bash to set permissions
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "chmod",
                    Arguments = $"-R 755 {path}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var process = new System.Diagnostics.Process { StartInfo = startInfo };
                process.Start();
                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    var error = await process.StandardError.ReadToEndAsync();
                    _logger.Warning("Failed to set Unix permissions: {Error}", error);
                }
                else
                {
                    _logger.Debug("Successfully set Unix permissions for: {Path}", path);
                }
            }
            catch (Exception ex)
            {
                _logger.Warning(ex, "Failed to set Unix permissions for {Path}", path);
                // Don't throw - this is not critical
            }
        }

        private static Credentials GetCredentials(GitAuth auth)
        {
            switch (auth.Type?.ToLower())
            {
                case "pat":
                    return new UsernamePasswordCredentials
                    {
                        Username = auth.Username ?? "git",
                        Password = auth.Token
                    };

                case "basic":
                    return new UsernamePasswordCredentials
                    {
                        Username = auth.Username,
                        Password = auth.Token
                    };

                default:
                    _logger.Debug("No authentication provided, using anonymous access");
                    return null;
            }
        }
    }
}