using beheersysteem_uitvaartcentrum.backend.api.Requests;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.Document;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.DossierFile;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using beheersysteem_uitvaartcentrum.backend.domain.Constanten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace beheersysteem_uitvaartcentrum.backend.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService dossierFileService)
        {
            _documentService = dossierFileService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            ViewDocumentDTO? document = await _documentService.GetDocumentAsync(id);

            if (document == null)
            {
                return NoContent();
            }

            return Ok(document);
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(Guid id)
        {
            DownloadDocumentDTO? downloadDocumentDTO = await _documentService.DownloadDocumentAsync(User, id);

            if (downloadDocumentDTO == null)
            {
                return NoContent();
            }

            return File(downloadDocumentDTO.Content, downloadDocumentDTO.ContentType, downloadDocumentDTO.FileName);
        }


        [HttpPost()]
        public async Task<IActionResult> Upload([FromForm] UploadDocumentRequest uploadDocumentRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // quick server-side validations for file size and extension
            string extension = Path.GetExtension(uploadDocumentRequest.File.FileName).ToLower();
            if (!Constanten.AllowedFileExtensions.Contains(extension))
            {
                return BadRequest(new { message = "Bestandstype niet toegestaan" });
            }

            if (uploadDocumentRequest.File.Length > Constanten.MaxFileSizeInBytes)
            {
                return BadRequest(new { message = $"Bestand is te groot. Maximaal toegestaan: {Constanten.MaxFileSizeInBytes} bytes" });
            }

            UploadDocumentDTO uploadDocumentDTO = new UploadDocumentDTO
            {
                DossierId = uploadDocumentRequest.DossierId!.Value,
                FileName = uploadDocumentRequest.File.FileName,
                Content = uploadDocumentRequest.File.OpenReadStream()
            };

            ViewDocumentDTO viewDocumentDTO = await _documentService.UploadDocumentAsync(User, uploadDocumentDTO);

            return CreatedAtAction(nameof(Get), new { id = viewDocumentDTO.Id }, viewDocumentDTO);
        }
    }
}
