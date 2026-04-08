using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using beheersysteem_uitvaartcentrum.backend.domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace beheersysteem_uitvaartcentrum.backend.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DossierController : ControllerBase
    {
        private readonly IDossierService _dossierService;

        public DossierController(IDossierService dossierService)
        {
            _dossierService = dossierService;
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            Dossier? dossierModel = await _dossierService.Get(id);

            if (dossierModel == null)
            {
                return NotFound();
            }

            ViewDossierDTO viewDossierDTO = new ViewDossierDTO
            {
                Id = dossierModel.Id,
                Title = dossierModel.Title,
                Description = dossierModel.Description,
                DateCreated = dossierModel.DateCreated,
                Files = dossierModel.Files
            };

            return Ok(viewDossierDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Dossier> dossiersModels = await _dossierService.GetAll();

            List<OverviewDossierDTO> overviewDossierDTOs = dossiersModels.Select(dossier => new OverviewDossierDTO
            {
                Id = dossier.Id,
                Title = dossier.Title
            }).ToList();

            return Ok(overviewDossierDTOs);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDossierDTO createDossierDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Dossier dossierModel = new Dossier
            {
                Id = Guid.NewGuid(),
                Title = createDossierDTO.Title,
                Description = createDossierDTO.Description,
                DateCreated = DateTime.UtcNow
            };

            Dossier createdDossierModel = await _dossierService.Create(dossierModel);

            ViewDossierDTO createdDossierDTO = new ViewDossierDTO
            {
                Id = createdDossierModel.Id,
                Title = createdDossierModel.Title,
                Description = createdDossierModel.Description,
                DateCreated = createdDossierModel.DateCreated
            };

            return CreatedAtAction(nameof(GetById), new { id = createdDossierDTO.Id }, createdDossierDTO);
        }
    }
}