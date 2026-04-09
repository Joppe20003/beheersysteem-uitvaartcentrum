namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories
{
    public interface IFileStorageProvider
    {
        public Task UploadDocumentAsync(Guid dossierId, string fileName, Stream content);
        public Task<Stream> DownloadDocumentAsync(Guid dossierId, string fileName);
    }
}
