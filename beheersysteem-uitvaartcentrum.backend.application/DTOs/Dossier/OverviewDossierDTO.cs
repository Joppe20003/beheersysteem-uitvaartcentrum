using beheersysteem_uitvaartcentrum.backend.domain.Models;

namespace beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier
{
    public class OverviewDossierDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
    }
}