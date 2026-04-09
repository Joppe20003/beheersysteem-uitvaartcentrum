namespace beheersysteem_uitvaartcentrum.backend.application.DTOs.Document
{
    public class ViewDocumentDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Extensions { get; set; } = string.Empty;
        public DateTime DateUploaded { get; set; }
    }
}
