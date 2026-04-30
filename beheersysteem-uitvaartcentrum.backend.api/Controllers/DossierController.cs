using beheersysteem_uitvaartcentrum.backend.api.Requests;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<IActionResult> GetAll()
        {
            List<OverviewDossierDTO> overviewDossierDTO = await _dossierService.GetAllDossiersAsync();
            List<OverviewDossierDTO> authorizedDossiers = new List<OverviewDossierDTO>();

            foreach (OverviewDossierDTO dossier in overviewDossierDTO)
            {
                AuthorizationResult? authorizationResult = await _authorizationService.AuthorizeAsync(User, dossier, "DossierOverviewAccess");

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

            AuthorizationResult? authorizationResult = await _authorizationService.AuthorizeAsync(User, createDossierDTO, "DossierCreate");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            string userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            ViewDossierDTO viewDossierDTO = await _dossierService.CreateDossierAsync(createDossierDTO, userId);

            return CreatedAtAction(nameof(GetById), new { id = viewDossierDTO.Id }, viewDossierDTO);
        }

    }
}