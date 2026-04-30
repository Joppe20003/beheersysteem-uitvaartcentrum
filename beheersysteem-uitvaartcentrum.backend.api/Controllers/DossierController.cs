using beheersysteem_uitvaartcentrum.backend.api.Requests;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace beheersysteem_uitvaartcentrum.backend.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DossierController : ControllerBase
    {
        private readonly IDossierService _dossierService;
        private readonly IAuthorizationService _authorizationService;

        public DossierController(IDossierService dossierService, IAuthorizationService authorizationService)
        {
            _dossierService = dossierService;
            _authorizationService = authorizationService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid dossierId)
        {
            ViewDossierDTO? viewDossierDTO = await _dossierService.GetDossierAsync(User, dossierId);

            if (viewDossierDTO == null)
            {
                return NoContent();
            }

            return Ok(viewDossierDTO);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<OverviewDossierDTO> dossiers = await _dossierService.GetAllDossiersAsync();

            return Ok(dossiers);
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

            ViewDossierDTO viewDossierDTO = await _dossierService.CreateDossierAsync(User, createDossierDTO);

            return CreatedAtAction(nameof(GetById), new { id = viewDossierDTO.Id }, viewDossierDTO);
        }

    }
}