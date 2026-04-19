using System.ComponentModel.DataAnnotations;

namespace beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier
{
    public class CreateDossierDTO
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
