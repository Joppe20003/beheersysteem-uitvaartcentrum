namespace beheersysteem_uitvaartcentrum.backend.application.DTOs.DossierFile
{
    public class UploadDocumentDTO
    {
        public Guid DossierId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public Stream Content { get; set; } = Stream.Null;
    }
}