using beheersysteem_uitvaartcentrum.backend.application.DTOs.Document;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.DossierFile;
using beheersysteem_uitvaartcentrum.backend.domain.Models;
using System.Security.Claims;

namespace beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services
{
    public interface IDocumentService
    {
        public Task<ViewDocumentDTO?> GetDocumentAsync(Guid id);
        public Task<ViewDocumentDTO> UploadDocumentAsync(ClaimsPrincipal user, UploadDocumentDTO uploadDocumentDTO);
        public Task<DownloadDocumentDTO?> DownloadDocumentAsync(ClaimsPrincipal user, Guid dossierId);
    }
}
