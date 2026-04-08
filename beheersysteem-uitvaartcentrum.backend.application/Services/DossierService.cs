using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using beheersysteem_uitvaartcentrum.backend.domain.Models;

namespace beheersysteem_uitvaartcentrum.backend.application.Services
{
    public class DossierService : IDossierService
    {
        private readonly IDossierRepository _dossierRepository;

        public DossierService(IDossierRepository dossierRepository)
        {
            _dossierRepository = dossierRepository;
        }

        public async Task<Dossier?> Get(Guid id)
        {
            return await _dossierRepository.Get(id);
        }

        public async Task<List<Dossier>> GetAll()
        {
            return await _dossierRepository.GetAll();
        }

        public async Task<Dossier> Create(Dossier dossier)
        {
            return await _dossierRepository.Create(dossier);
        }
    }
}
