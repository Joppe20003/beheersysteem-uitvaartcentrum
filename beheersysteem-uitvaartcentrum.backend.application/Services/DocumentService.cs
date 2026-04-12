using beheersysteem_uitvaartcentrum.backend.application.DTOs.Document;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.DossierFile;
using beheersysteem_uitvaartcentrum.backend.application.Exceptions;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using beheersysteem_uitvaartcentrum.backend.domain.Constanten;
using beheersysteem_uitvaartcentrum.backend.domain.Models;

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

    public async Task<ViewDocumentDTO> UploadDocumentAsync(UploadDocumentDTO dto)
    {
        await CheckDossierIdExists(dto.DossierId);

        await _fileStorageProvider.CheckExstensionIsAllowed(dto.FileName);
        await _fileStorageProvider.UploadDocumentAsync(dto.DossierId, dto.FileName, dto.Content, Constanten.AllowedFileExtensions);

        DocumentModel? existing = await _documentRepository.GetDocumentByDossierAndNameAsync(dto.DossierId, dto.FileName);

        if (existing != null)
        {
            return new ViewDocumentDTO
            {
                Id = existing.Id,
                Title = existing.Title,
                Extensions = existing.Extensions,
                DateUploaded = existing.DateUploaded
            };
        }

        var model = new DocumentModel
        {
            Id = Guid.NewGuid(),
            DossierId = dto.DossierId,
            Title = dto.FileName,
            Extensions = Path.GetExtension(dto.FileName),
            DateUploaded = DateTime.UtcNow
        };

        var created = await _documentRepository.CreateDocumentAsync(model);

        return new ViewDocumentDTO
        {
            Id = created.Id,
            Title = created.Title,
            Extensions = created.Extensions,
            DateUploaded = created.DateUploaded
        };
    }

    public async Task<DownloadDocumentDTO?> DownloadDocumentAsync(Guid id)
    {
        var doc = await _documentRepository.GetDocumentAsync(id);

        if (doc == null) return null;

        var stream = await _fileStorageProvider.DownloadDocumentAsync(doc.DossierId, doc.Title);

        return new DownloadDocumentDTO
        {
            FileName = doc.Title,
            ContentType = _fileStorageProvider.GetContentType(doc.Extensions),
            Content = stream
        };
    }

    private async Task CheckDossierIdExists(Guid dossierId)
    {
        ViewDossierDTO? dossierViewDTO = await _dossierService.GetDossierAsync(dossierId);

        if (dossierViewDTO == null)
        {
            throw new NotFoundForeignKey("Dossier niet gevonden", $"Dossier met het ID {dossierId} bestaat niet.");
        }
    }
}