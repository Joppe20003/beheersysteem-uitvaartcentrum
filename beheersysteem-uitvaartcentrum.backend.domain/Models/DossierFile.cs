namespace beheersysteem_uitvaartcentrum.backend.domain.Models
{
    public class DossierFile
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Extensions { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public Dossier Dossier { get; set; } = null!;
    }
}
