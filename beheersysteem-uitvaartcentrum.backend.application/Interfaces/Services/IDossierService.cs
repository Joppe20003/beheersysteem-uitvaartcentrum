using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using beheersysteem_uitvaartcentrum.backend.domain.Models;

namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services
{
    public interface IDossierService
    {
        Task<ViewDossierDTO?> GetDossierAsync(Guid id);
        Task<List<OverviewDossierDTO>> GetAllDossiersAsync();
        Task<ViewDossierDTO> CreateDossierAsync(CreateDossierDTO dto);
    }
}
