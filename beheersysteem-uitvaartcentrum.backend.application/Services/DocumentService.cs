using beheersysteem_uitvaartcentrum.backend.application.DTOs.Document;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.DossierFile;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using beheersysteem_uitvaartcentrum.backend.domain.Models;

public class DocumentService : IDocumentService
{
    private readonly IFileStorageProvider _fileStorageProvider;
    private readonly IDocumentRepository _documentRepository;

    public DocumentService(IFileStorageProvider fileStorageProvider, IDocumentRepository documentRepository)
    {
        _fileStorageProvider = fileStorageProvider;
        _documentRepository = documentRepository;
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
        await _fileStorageProvider.UploadDocumentAsync(dto.DossierId, dto.FileName, dto.Content);

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
            ContentType = GetContentType(doc.Extensions),
            Content = stream
        };
    }

    private string GetContentType(string extension)
    {
        return extension.ToLower() switch
        {
            ".pdf" => "application/pdf",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "application/octet-stream"
        };
    }
}