using beheersysteem_uitvaartcentrum.backend.domain.Models;

namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services
{
    public interface IDossierService
    {
        public Task<Dossier?> Get(Guid id);
        public Task<List<Dossier>> GetAll();
        public Task<Dossier> Create(Dossier dossier);
    }
}
