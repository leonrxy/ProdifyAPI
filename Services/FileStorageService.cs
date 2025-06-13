using Microsoft.Extensions.Options;

public class FileStorageOptions
{
    public string WebRootPath { get; set; } = "wwwroot";      // biasanya wwwroot
    public string UploadsFolder { get; set; } = "uploads";     // contoh: "uploads/products"
}

public interface IFileStorageService
{

    Task<string?> SaveFileAsync(IFormFile file, string subFolder);
    Task DeleteFileAsync(string relativePath);
    Task<string?> ReplaceFileAsync(string? oldRelativePath, IFormFile newFile, string subFolder);
}

public class FileStorageService : IFileStorageService
{
    private readonly FileStorageOptions _opts;
    public FileStorageService(IOptions<FileStorageOptions> opts)
        => _opts = opts.Value;

    private string GetAbsoluteFolder(string subFolder)
    {
        var folder = Path.Combine(_opts.WebRootPath, _opts.UploadsFolder, subFolder);
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        return folder;
    }

    public async Task<string?> SaveFileAsync(IFormFile file, string subFolder)
    {
        if (file == null || file.Length == 0) return null;

        var folder = GetAbsoluteFolder(subFolder);
        var uniqueName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var fullPath = Path.Combine(folder, uniqueName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        var relative = Path.Combine(_opts.UploadsFolder, subFolder, uniqueName)
                           .Replace("\\", "/");
        return "/" + relative;
    }

    public Task DeleteFileAsync(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath)) return Task.CompletedTask;

        var trimmed = relativePath.TrimStart('/');
        var fullPath = Path.Combine(_opts.WebRootPath, trimmed);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        return Task.CompletedTask;
    }

    public async Task<string?> ReplaceFileAsync(string? oldRelativePath, IFormFile newFile, string subFolder)
    {
        await DeleteFileAsync(oldRelativePath);
        return await SaveFileAsync(newFile, subFolder);
    }
}