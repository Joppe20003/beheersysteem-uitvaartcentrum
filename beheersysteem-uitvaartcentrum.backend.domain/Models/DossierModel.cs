namespace beheersysteem_uitvaartcentrum.backend.domain.Models
{
    public class DossierModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<Guid> InvitedUserIds { get; set; } = new List<Guid>();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public List<DocumentModel> Documents { get; set; } = new List<DocumentModel>();
    }
}
