using beheersysteem_uitvaartcentrum.backend.application.DTOs.Document;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.DossierFile;
using beheersysteem_uitvaartcentrum.backend.domain.Models;

namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services
{
    public interface IDocumentService
    {
        public Task<ViewDocumentDTO?> GetDocumentAsync(Guid id);
        public Task<ViewDocumentDTO> UploadDocumentAsync(UploadDocumentDTO uploadDossierFileDTO);
        public Task<DownloadDocumentDTO?> DownloadDocumentAsync(Guid id);
    }
}
