namespace beheersysteem_uitvaartcentrum.backend.domain.Models
{
    public class DossierFile
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid DossierId { get; set; }
        public string Extensions { get; set; } = string.Empty;
        public DateTime DateUploaded { get; set; }
        public Dossier Dossier { get; set; } = null!;
    }
}
