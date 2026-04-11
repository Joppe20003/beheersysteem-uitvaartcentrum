using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using beheersysteem_uitvaartcentrum.backend.domain.Constanten;
using Microsoft.Extensions.Configuration;

public class FileStorageProvider : IFileStorageProvider
{
    private readonly string? _uploadRoot;

    public FileStorageProvider(IConfiguration configuration)
    {
        _uploadRoot = configuration["FileStorage:BasePath"];
    }

    public async Task UploadDocumentAsync(Guid dossierId, string fileName, Stream content, List<string> allowedExtensions)
    {
        string storagePath = GetStoragePath(dossierId, fileName);

        Directory.CreateDirectory(Path.GetDirectoryName(storagePath)!);

        using var fileStream = new FileStream(storagePath, FileMode.Create, FileAccess.Write);

        await content.CopyToAsync(fileStream);
    }

    public Task<Stream> DownloadDocumentAsync(Guid dossierId, string fileName)
    {
        string path = GetStoragePath(dossierId, fileName);

        if (!File.Exists(path))
            throw new FileNotFoundException();

        Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        return Task.FromResult(stream);
    }

    public string GetContentType(string extension)
    {
        return extension.ToLower() switch
        {
            ".pdf" => "application/pdf",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream"
        };
    }


    public async Task CheckExstensionIsAllowed(string fileName)
    {
        string extension = Path.GetExtension(fileName).ToLower();

        if (!Constanten.AllowedFileExtensions.Contains(extension))
        {
            throw new ArgumentException($"Bestandstype '{extension}' is niet toegestaan.");
        }
    }

    private string GetStoragePath(Guid dossierId, string fileName)
    {
        string folder = Path.Combine(_uploadRoot, dossierId.ToString());
        return Path.Combine(folder, fileName);
    }
}