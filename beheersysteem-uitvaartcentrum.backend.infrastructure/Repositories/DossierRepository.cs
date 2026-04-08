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

        public async Task<Dossier?> Get(Guid id)
        {
            return await _appDbContext.Dossiers.Include(d => d.Files).FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<Dossier>> GetAll()
        {
            return await _appDbContext.Dossiers.ToListAsync();
        }

        public async Task<Dossier> Create(Dossier dossier)
        {
            _appDbContext.Dossiers.Add(dossier);

            await _appDbContext.SaveChangesAsync();

            return dossier;
        }
    }
}
