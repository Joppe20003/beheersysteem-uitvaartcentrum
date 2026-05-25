using System.ComponentModel.DataAnnotations;

namespace beheersysteem_uitvaartcentrum.backend.api.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is verplicht.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        public string Password { get; set; } = string.Empty;
    }
}
