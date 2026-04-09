using System.ComponentModel.DataAnnotations;

namespace beheersysteem_uitvaartcentrum.backend.api.Requests
{
    public class UploadDocumentRequest
    {
        [Required(ErrorMessage = "Dossier ID is verplicht")]
        public Guid DossierId { get; set; }

        [Required(ErrorMessage = "Bestand is verplicht")]
        public IFormFile File { get; set; } = null!;
    }
}
