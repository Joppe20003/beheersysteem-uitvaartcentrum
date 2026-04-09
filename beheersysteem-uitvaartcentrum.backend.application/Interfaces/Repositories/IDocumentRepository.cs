using beheersysteem_uitvaartcentrum.backend.domain.Models;

namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories
{
    public interface IDocumentRepository
    {
        public Task<DocumentModel?> GetDocumentAsync(Guid id);
        public Task<DocumentModel> CreateDocumentAsync(DocumentModel dossierFileModel);
    }
}
