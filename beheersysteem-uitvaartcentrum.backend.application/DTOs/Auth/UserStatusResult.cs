namespace beheersysteem_uitvaartcentrum.backend.application.DTOs.Auth
{
    public class UserStatusResult
    {
        public bool IsAuthenticated { get; set; }
        public string? Username { get; set; }
        public string? UserId { get; set; }
        public IEnumerable<string>? Roles { get; set; }
        public IEnumerable<string>? Actions { get; set; }
    }
}
