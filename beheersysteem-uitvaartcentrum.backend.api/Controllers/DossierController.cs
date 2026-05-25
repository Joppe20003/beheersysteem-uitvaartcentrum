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

        public DossierController(IDossierService dossierService)
        {
            _dossierService = dossierService;
        }

        [HttpGet("{dossierId}")]
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
            List<OverviewDossierDTO> dossiers = await _dossierService.GetAllDossiersAsync(User);

            return Ok(dossiers);
        }

        [HttpPost("create")]
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

            return CreatedAtAction(nameof(GetById), new { dossierId = viewDossierDTO.Id }, viewDossierDTO);
        }

        [HttpPost("invite")]
        public async Task<IActionResult> Invite([FromBody] InviteDossierRequest inviteDossierRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _dossierService.InviteUserToDossierAsync(User, inviteDossierRequest.DossierId, inviteDossierRequest.TargetedUserId);

            return NoContent();
        }
    }
}