using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using System.Security.Claims;

namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services
{
    public interface IDossierService
    {
        Task<ViewDossierDTO?> GetDossierAsync(ClaimsPrincipal user, Guid dossierId);
        Task<List<OverviewDossierDTO>> GetAllDossiersAsync();
        Task<ViewDossierDTO> CreateDossierAsync(ClaimsPrincipal user, CreateDossierDTO createDossierDTO);
    }
}
