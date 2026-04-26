using beheersysteem_uitvaartcentrum.backend.api.Requests;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace beheersysteem_uitvaartcentrum.backend.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public async Task<IActionResult> GetById(Guid id)
        {
            ViewDossierDTO? viewDossierDTO = await _dossierService.GetDossierAsync(id);

            if (viewDossierDTO == null)
            {
                return NoContent();
            }

            return Ok(viewDossierDTO);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            List<OverviewDossierDTO> overviewDossierDTO = await _dossierService.GetAllDossiersAsync();
            List<OverviewDossierDTO> authorizedDossiers = new List<OverviewDossierDTO>();

            foreach (OverviewDossierDTO dossier in overviewDossierDTO)
            {
                var authorizationResult = await _authorizationService.AuthorizeAsync(User, dossier, "DossierAccess");

                if (authorizationResult.Succeeded)
                {
                    authorizedDossiers.Add(dossier);
                }
            }

            var result = authorizedDossiers.Select(dossier => new
            {
                dossier.Id,
                dossier.Title
            });

            return Ok(result);
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
            
            ViewDossierDTO viewDossierDTO = await _dossierService.CreateDossierAsync(createDossierDTO);

            return CreatedAtAction(nameof(GetById), new { id = viewDossierDTO.Id }, viewDossierDTO);
        }

    }
}