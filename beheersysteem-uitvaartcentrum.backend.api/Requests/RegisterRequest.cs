using System.ComponentModel.DataAnnotations;
using beheersysteem_uitvaartcentrum.backend.domain.Enums;

namespace beheersysteem_uitvaartcentrum.backend.api.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Gebruikersnaam is verplicht.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rol is verplicht.")]
        public RolesRequest? Role { get; set; }

        [Required(ErrorMessage = "Email is verplicht.")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        [MinLength(16, ErrorMessage = "Wachtwoord moet minimaal 16 tekens bevatten.")]
        public string Password { get; set; } = string.Empty;
    }
}
