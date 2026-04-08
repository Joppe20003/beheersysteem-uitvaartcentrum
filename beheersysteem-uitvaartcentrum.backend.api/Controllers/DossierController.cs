using beheersysteem_uitvaartcentrum.backend.api.Requests;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            ViewDossierDTO? viewDossierDTO = await _dossierService.GetAsync(id);

            if (viewDossierDTO == null)
            {
                return NoContent();
            }

            return Ok(viewDossierDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<OverviewDossierDTO> overviewDossierDTO = await _dossierService.GetAllAsync();

            return Ok(overviewDossierDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDossierRequest createDossierRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateDossierDTO createDossierDTO = new CreateDossierDTO
            {
                Title = createDossierRequest.Title,
                Description = createDossierRequest.Description
            };
            
            ViewDossierDTO viewDossierDTO = await _dossierService.CreateAsync(createDossierDTO);

            return CreatedAtAction(nameof(GetById), new { id = viewDossierDTO.Id }, viewDossierDTO);
        }

    }
}