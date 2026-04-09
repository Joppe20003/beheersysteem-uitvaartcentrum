using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using beheersysteem_uitvaartcentrum.backend.domain.Models;
using beheersysteem_uitvaartcentrum.backend.infrastructure.Data;

namespace beheersysteem_uitvaartcentrum.backend.infrastructure.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly AppDbContext _appDbContext;

        public DocumentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<DocumentModel?> GetDocumentAsync(Guid id)
        {
            return await _appDbContext.Documents.FindAsync(id);
        }


        public async Task<DocumentModel> CreateDocumentAsync(DocumentModel file)
        {
            _appDbContext.Documents.Add(file);

            await _appDbContext.SaveChangesAsync();

            return file;
        }
    }
}
