using beheersysteem_uitvaartcentrum.backend.domain.Models;

namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories
{
    public interface IDocumentRepository
    {
        public Task<DocumentModel?> getAsync(Guid id);
        public Task<DocumentModel> createAsync(DocumentModel dossierFileModel);
    }
}
