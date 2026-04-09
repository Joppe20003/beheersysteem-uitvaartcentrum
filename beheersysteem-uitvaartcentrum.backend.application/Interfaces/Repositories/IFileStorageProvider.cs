namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories
{
    public interface IFileStorageProvider
    {
        public Task uploadDocumentAsync(Guid dossierId, string fileName, Stream content);
        public Task<Stream> downloadDocumentAsync(Guid dossierId, string fileName);
    }
}
