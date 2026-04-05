namespace beheersysteem_uitvaartcentrum.backend.domain.Models
{
    public class Dossier
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public List<DossierFile> File { get; set; } = new List<DossierFile>();
    }
}
