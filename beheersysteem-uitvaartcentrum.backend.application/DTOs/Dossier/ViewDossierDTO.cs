using beheersysteem_uitvaartcentrum.backend.application.DTOs.Document;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.User;

namespace beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier
{
    public class ViewDossierDTO
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime DateCreated { get; set; }

        public List<ViewDocumentDTO> Documents { get; set; } = null!;

        public List<InvitedUserDTO> InvitedUsers { get; set; } = null!;
    }
}