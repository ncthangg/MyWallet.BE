using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CocoQR.Application.Contracts.ISubServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CocoQR.Infrastructure.SubService
{
    public class CloudinaryStorage : ICloudStorage
    {
        private const string DeliveryTypeSegment = "upload";

        private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp", ".tiff", ".svg", ".avif"
        };

        private static readonly HashSet<string> VideoExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".mp4", ".mov", ".avi", ".wmv", ".mkv", ".webm", ".m4v", ".flv"
        };

        private readonly CloudinarySettings _settings;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger<CloudinaryStorage> _logger;
        private readonly bool _isValid;

        public CloudinaryStorage(
            IOptions<CloudinarySettings> options,
            IHostEnvironment hostEnvironment,
            ILogger<CloudinaryStorage> logger)
        {
            _settings = options.Value;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
            _isValid = ValidateSettings();
        }

        public async Task UploadAsync(Stream stream, string path)
        {
            EnsureConfigured();

            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var storagePath = BuildStoragePath(path);
            if (string.IsNullOrWhiteSpace(storagePath))
                throw new ArgumentException("Path is required", nameof(path));

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            var client = GetClient();
            var publicId = GetPublicId(storagePath);
            var resourceType = ResolveResourceType(storagePath);

            UploadResult uploadResult = resourceType switch
            {
                ResourceType.Image => await client.UploadAsync(new ImageUploadParams
                {
                    File = new FileDescription(Path.GetFileName(storagePath), stream),
                    PublicId = publicId,
                    Overwrite = true,
                    UniqueFilename = false,
                    UseFilename = false
                }),
                ResourceType.Video => await client.UploadAsync(new VideoUploadParams
                {
                    File = new FileDescription(Path.GetFileName(storagePath), stream),
                    PublicId = publicId,
                    Overwrite = true,
                    UniqueFilename = false,
                    UseFilename = false
                }),
                _ => await client.UploadAsync(new RawUploadParams
                {
                    File = new FileDescription(Path.GetFileName(storagePath), stream),
                    PublicId = publicId,
                    Overwrite = true,
                    UniqueFilename = false,
                    UseFilename = false
                })
            };

            if (uploadResult.Error != null)
            {
                throw new InvalidOperationException($"Cloudinary upload failed: {uploadResult.Error.Message}");
            }
        }

        public async Task DeleteAsync(string path)
        {
            EnsureConfigured();

            var storagePath = BuildStoragePath(path);
            if (string.IsNullOrWhiteSpace(storagePath))
                return;

            var deletionParams = new DeletionParams(GetPublicId(storagePath))
            {
                ResourceType = ResolveResourceType(storagePath)
            };

            var deletionResult = await GetClient().DestroyAsync(deletionParams);
            if (deletionResult.Error != null)
            {
                throw new InvalidOperationException($"Cloudinary delete failed: {deletionResult.Error.Message}");
            }
        }

        public string GetPublicUrl(string path)
        {
            var storagePath = BuildStoragePath(path);
            if (string.IsNullOrWhiteSpace(storagePath))
                return string.Empty;

            var resourceTypeSegment = GetResourceTypeSegment(ResolveResourceType(storagePath));
            return $"{GetPublicBaseUrl()}/{resourceTypeSegment}/{DeliveryTypeSegment}/{storagePath}";
        }

        private void EnsureConfigured()
        {
            if (!_isValid)
            {
                throw new InvalidOperationException("Cloudinary storage is not configured correctly.");
            }
        }

        private bool ValidateSettings()
        {
            var isValid = true;

            if (string.IsNullOrWhiteSpace(_settings.CloudName))
            {
                _logger.LogError("Cloudinary CloudName missing");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
            {
                _logger.LogError("Cloudinary ApiKey missing");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(_settings.ApiSecret))
            {
                _logger.LogError("Cloudinary ApiSecret missing");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(_settings.ProjectName))
            {
                _logger.LogError("Cloudinary ProjectName missing");
                isValid = false;
            }

            if (!isValid)
            {
                _logger.LogWarning("Cloudinary storage disabled due to invalid config");
            }

            return isValid;
        }

        private Cloudinary GetClient()
        {
            var account = new Account(_settings.CloudName, _settings.ApiKey, _settings.ApiSecret);
            return new Cloudinary(account);
        }

        private string BuildStoragePath(string path)
        {
            var normalizedPath = NormalizePath(path);
            if (string.IsNullOrWhiteSpace(normalizedPath))
            {
                return string.Empty;
            }

            var envSegment = NormalizePath(_hostEnvironment.EnvironmentName.ToLowerInvariant());
            var projectSegment = NormalizePath(_settings.ProjectName);

            if (!string.IsNullOrWhiteSpace(envSegment)
                && !normalizedPath.StartsWith($"{envSegment}/", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(normalizedPath, envSegment, StringComparison.OrdinalIgnoreCase))
            {
                normalizedPath = $"{envSegment}/{normalizedPath}";
            }

            if (!string.IsNullOrWhiteSpace(projectSegment)
                && !normalizedPath.StartsWith($"{projectSegment}/", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(normalizedPath, projectSegment, StringComparison.OrdinalIgnoreCase))
            {
                normalizedPath = $"{projectSegment}/{normalizedPath}";
            }

            return normalizedPath;
        }

        private string GetPublicBaseUrl()
        {
            var cloudName = NormalizePath(_settings.CloudName);
            var configuredBaseUrl = (_settings.BaseUrl ?? string.Empty).Trim().TrimEnd('/');
            if (string.IsNullOrWhiteSpace(configuredBaseUrl))
            {
                return $"https://res.cloudinary.com/{cloudName}";
            }

            if (!Uri.TryCreate(configuredBaseUrl, UriKind.Absolute, out var baseUri))
            {
                return $"https://res.cloudinary.com/{cloudName}";
            }

            var segments = baseUri.AbsolutePath
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .Select(NormalizePath)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();

            if (segments.Count >= 2
                && string.Equals(segments[^1], DeliveryTypeSegment, StringComparison.OrdinalIgnoreCase)
                && IsResourceTypeSegment(segments[^2]))
            {
                segments.RemoveRange(segments.Count - 2, 2);
            }

            if (segments.Count == 0 || !string.Equals(segments[0], cloudName, StringComparison.OrdinalIgnoreCase))
            {
                segments.Insert(0, cloudName);
            }

            var builder = new UriBuilder(baseUri)
            {
                Path = string.Join('/', segments),
                Query = string.Empty,
                Fragment = string.Empty
            };

            return builder.Uri.ToString().TrimEnd('/');
        }

        private static bool IsResourceTypeSegment(string segment)
        {
            return string.Equals(segment, "raw", StringComparison.OrdinalIgnoreCase)
                || string.Equals(segment, "image", StringComparison.OrdinalIgnoreCase)
                || string.Equals(segment, "video", StringComparison.OrdinalIgnoreCase);
        }

        private static ResourceType ResolveResourceType(string path)
        {
            var extension = Path.GetExtension(path);
            if (string.IsNullOrWhiteSpace(extension))
            {
                return ResourceType.Raw;
            }

            if (ImageExtensions.Contains(extension))
            {
                return ResourceType.Image;
            }

            if (VideoExtensions.Contains(extension))
            {
                return ResourceType.Video;
            }

            return ResourceType.Raw;
        }

        private static string GetResourceTypeSegment(ResourceType resourceType)
        {
            return resourceType switch
            {
                ResourceType.Image => "image",
                ResourceType.Video => "video",
                _ => "raw"
            };
        }

        private static string GetPublicId(string path)
        {
            var extension = Path.GetExtension(path);
            return string.IsNullOrWhiteSpace(extension)
                ? path
                : path[..^extension.Length];
        }

        private static string NormalizePath(string path)
        {
            return path
                .Replace('\\', '/')
                .TrimStart('/');
        }
    }
}