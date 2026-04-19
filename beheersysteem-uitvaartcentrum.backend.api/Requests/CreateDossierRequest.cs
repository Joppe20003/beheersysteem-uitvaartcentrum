using System.ComponentModel.DataAnnotations;

namespace beheersysteem_uitvaartcentrum.backend.api.Requests
{
    public class CreateDossierRequest
    {
        [Required(ErrorMessage = "Titel is verplicht")]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
