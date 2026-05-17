namespace beheersysteem_uitvaartcentrum.backend.application.DTOs.Auth
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
