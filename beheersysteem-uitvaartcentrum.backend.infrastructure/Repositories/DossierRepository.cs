using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using beheersysteem_uitvaartcentrum.backend.domain.Models;
using beheersysteem_uitvaartcentrum.backend.infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace beheersysteem_uitvaartcentrum.backend.infrastructure.Repositories
{
    public class DossierRepository : IDossierRepository
    {
        private readonly AppDbContext _appDbContext;

        public DossierRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<DossierModel?> getAsync(Guid id)
        {
            return await _appDbContext.Dossiers.Include(d => d.Documents).FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<DossierModel>> getAllAsync()
        {
            return await _appDbContext.Dossiers.ToListAsync();
        }

        public async Task<DossierModel> createAsync(DossierModel dossier)
        {
            _appDbContext.Dossiers.Add(dossier);

            await _appDbContext.SaveChangesAsync();

            return dossier;
        }
    }
}
