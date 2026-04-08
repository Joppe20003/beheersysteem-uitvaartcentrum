using beheersysteem_uitvaartcentrum.backend.application.DTOs.Document;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.DossierFile;
using beheersysteem_uitvaartcentrum.backend.domain.Models;

namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services
{
    public interface IDocumentService
    {
        public Task<ViewDocumentDTO?> getAsync(Guid id);
        public Task<ViewDocumentDTO> uploadDocumentAsync(UploadDocumentDTO uploadDossierFileDTO);
        public Task<DownloadDocumentDTO?> downloadDocumentAsync(Guid id);
    }
}
