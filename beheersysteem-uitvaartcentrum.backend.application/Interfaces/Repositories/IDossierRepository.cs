using beheersysteem_uitvaartcentrum.backend.domain.Models;

namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories
{
    public interface IDossierRepository
    {
        public Task<DossierModel?> GetDossierAsync(Guid id);
        public Task<List<DossierModel>> GetAllDossiersAsync();
        public Task<DossierModel> CreateDossierAsync(DossierModel dossier);
    }
}
