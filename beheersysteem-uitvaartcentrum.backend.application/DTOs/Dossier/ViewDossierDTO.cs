using beheersysteem_uitvaartcentrum.backend.domain.Models;

namespace beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier
{
    public class ViewDossierDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime DateCreated { get; set; }

        public List<DossierFile>? Files { get; set; }
    }
}