using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;

public class FileStorageProvider : IFileStorageProvider
{
    private readonly string _uploadRoot;

    public FileStorageProvider(IConfiguration configuration)
    {
        _uploadRoot = configuration["FileStorage:BasePath"];
    }

    public async Task uploadDocumentAsync(Guid dossierId, string fileName, Stream content)
    {
        string storagePath = GetStoragePath(dossierId, fileName);

        Directory.CreateDirectory(Path.GetDirectoryName(storagePath)!);

        using var fileStream = new FileStream(storagePath, FileMode.Create, FileAccess.Write);

        await content.CopyToAsync(fileStream);
    }

    public Task<Stream> downloadDocumentAsync(Guid dossierId, string fileName)
    {
        string path = GetStoragePath(dossierId, fileName);

        if (!File.Exists(path))
            throw new FileNotFoundException();

        Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        return Task.FromResult(stream);
    }

    private string GetStoragePath(Guid dossierId, string fileName)
    {
        string folder = Path.Combine(_uploadRoot, dossierId.ToString());
        return Path.Combine(folder, fileName);
    }
}