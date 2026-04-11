using beheersysteem_uitvaartcentrum.backend.api.Requests;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.Document;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.DossierFile;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace beheersysteem_uitvaartcentrum.backend.api.Controllers
{
    [Route("[controller]")]
    [ApiController]
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
            DownloadDocumentDTO? downloadDocumentDTO = await _documentService.DownloadDocumentAsync(id);

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

            UploadDocumentDTO uploadDocumentDTO = new UploadDocumentDTO
            {
                DossierId = uploadDocumentRequest.DossierId!.Value,
                FileName = uploadDocumentRequest.File.FileName,
                Content = uploadDocumentRequest.File.OpenReadStream()
            };

            ViewDocumentDTO viewDocumentDTO = await _documentService.UploadDocumentAsync(uploadDocumentDTO);

            return CreatedAtAction(nameof(Get), new { id = viewDocumentDTO.Id }, viewDocumentDTO);
        }
    }
}
