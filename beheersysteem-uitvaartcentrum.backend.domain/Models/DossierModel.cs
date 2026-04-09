namespace beheersysteem_uitvaartcentrum.backend.domain.Models
{
    public class DossierModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public List<DocumentModel> Documents { get; set; } = new List<DocumentModel>();
    }
}
