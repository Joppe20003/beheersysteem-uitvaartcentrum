namespace beheersysteem_uitvaartcentrum.backend.domain.Models
{
    public class DossierInvitedModel
    {
        public Guid DossierId { get; set; }
        public DossierModel Dossier { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
