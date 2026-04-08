using System.ComponentModel.DataAnnotations;

namespace beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier
{
    public class CreateDossierDTO
    {
        [Required(ErrorMessage = "Titel is verplicht")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
