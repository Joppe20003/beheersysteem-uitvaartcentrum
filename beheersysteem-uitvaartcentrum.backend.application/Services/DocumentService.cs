using beheersysteem_uitvaartcentrum.backend.application.DTOs.Document;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.DossierFile;
using beheersysteem_uitvaartcentrum.backend.application.Exceptions;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using beheersysteem_uitvaartcentrum.backend.domain.Constanten;
using beheersysteem_uitvaartcentrum.backend.domain.Models;
using System.Security.Claims;

public class DocumentService : IDocumentService
{
    private readonly IFileStorageProvider _fileStorageProvider;
    private readonly IDocumentRepository _documentRepository;
    private readonly IDossierService _dossierService;

    public DocumentService(IFileStorageProvider fileStorageProvider, IDocumentRepository documentRepository, IDossierService dossierService)
    {
        _fileStorageProvider = fileStorageProvider;
        _documentRepository = documentRepository;
        _dossierService = dossierService;
    }


    public async Task<ViewDocumentDTO?> GetDocumentAsync(Guid id)
    {
        DocumentModel? documentModel = await _documentRepository.GetDocumentAsync(id);

        if (documentModel == null) return null;

        ViewDocumentDTO viewDocumentDTO = new ViewDocumentDTO
        {
            Id = documentModel.Id,
            Title = documentModel.Title,
            Extensions = documentModel.Extensions,
            DateUploaded = documentModel.DateUploaded
        };

        return viewDocumentDTO;
    }

    public async Task<ViewDocumentDTO> UploadDocumentAsync(ClaimsPrincipal user, UploadDocumentDTO uploadDocumentDTO)
    {
        string userId = user.FindFirst(claim => claim.Type == "userId")?.Value;

        await CheckDossierIdExists(user, uploadDocumentDTO.DossierId);

        await _fileStorageProvider.CheckExstensionIsAllowed(uploadDocumentDTO.FileName);

        await _fileStorageProvider.UploadDocumentAsync(uploadDocumentDTO.DossierId, uploadDocumentDTO.FileName, uploadDocumentDTO.Content, Constanten.AllowedFileExtensions);

        DocumentModel? existing = await _documentRepository.GetDocumentByDossierAndNameAsync(uploadDocumentDTO.DossierId, uploadDocumentDTO.FileName);

        if (existing != null)
        {
            existing.DateUploaded = DateTime.UtcNow;

            await _documentRepository.UpdateDocumentAsync(existing);

            return new ViewDocumentDTO
            {
                Id = existing.Id,
                Title = existing.Title,
                Extensions = existing.Extensions,
                DateUploaded = existing.DateUploaded
            };
        }

        DocumentModel model = new DocumentModel
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            DossierId = uploadDocumentDTO.DossierId,
            Title = uploadDocumentDTO.FileName,
            Extensions = Path.GetExtension(uploadDocumentDTO.FileName),
            DateUploaded = DateTime.UtcNow
        };

        DocumentModel created = await _documentRepository.CreateDocumentAsync(model);

        return new ViewDocumentDTO
        {
            Id = created.Id,
            Title = created.Title,
            Extensions = created.Extensions,
            DateUploaded = created.DateUploaded
        };
    }

    public async Task<DownloadDocumentDTO?> DownloadDocumentAsync(ClaimsPrincipal user, Guid id)
    {
        DocumentModel? documentModel = await _documentRepository.GetDocumentAsync(id);

        if (documentModel == null) return null;

        Stream stream = await _fileStorageProvider.DownloadDocumentAsync(documentModel.DossierId, documentModel.Title);

        return new DownloadDocumentDTO
        {
            FileName = documentModel.Title,
            ContentType = _fileStorageProvider.GetContentType(documentModel.Extensions),
            Content = stream
        };
    }

    private async Task CheckDossierIdExists(ClaimsPrincipal user, Guid dossierId)
    {
        ViewDossierDTO? dossierViewDTO = await _dossierService.GetDossierAsync(user, dossierId);

        if (dossierViewDTO == null)
        {
            throw new NotFoundForeignKey("Dossier niet gevonden", $"Dossier met het ID {dossierId} bestaat niet.");
        }
    }
}