using beheersysteem_uitvaartcentrum.backend.domain.Models;

namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories
{
    public interface IDossierRepository
    {
        public Task<DossierModel?> getAsync(Guid id);
        public Task<List<DossierModel>> getAllAsync();
        public Task<DossierModel> createAsync(DossierModel dossier);
    }
}
