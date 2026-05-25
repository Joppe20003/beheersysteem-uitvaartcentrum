using System.ComponentModel.DataAnnotations;

namespace beheersysteem_uitvaartcentrum.backend.api.Requests
{
    public class InviteDossierRequest
    {
        [Required(ErrorMessage = "Dossier ID is verplicht.")]
        public Guid DossierId { get; set; }

        [Required(ErrorMessage = "User ID is verplicht.")]
        public Guid TargetedUserId { get; set; }
    }
}
