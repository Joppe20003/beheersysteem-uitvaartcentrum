namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories
{
    public interface IFileStorageProvider
    {
        public Task UploadDocumentAsync(Guid dossierId, string fileName, Stream content, List<string> allowedExtensions);
        public Task<Stream> DownloadDocumentAsync(Guid dossierId, string fileName);
        public string GetContentType(string extension);
        public Task CheckExstensionIsAllowed(string fileName);
    }
}
