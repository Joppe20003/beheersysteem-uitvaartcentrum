using beheersysteem_uitvaartcentrum.backend.application.DTOs.Document;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.User;
using beheersysteem_uitvaartcentrum.backend.application.Exceptions;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using beheersysteem_uitvaartcentrum.backend.domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace beheersysteem_uitvaartcentrum.backend.application.Services
{
    public class DossierService : IDossierService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IDossierRepository _dossierRepository;
        private readonly IAuthorizationService _authorizationService;

        public DossierService(UserManager<IdentityUser> userManager, IDossierRepository dossierRepository, IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _dossierRepository = dossierRepository;
            _authorizationService = authorizationService;
        }
        public async Task<ViewDossierDTO?> GetDossierAsync(ClaimsPrincipal user, Guid dossierId)
        {
            DossierModel? dossierModel = await _dossierRepository.GetDossierAsync(dossierId);
            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(user, dossierModel, "DossierAccess");

            if (dossierModel == null) return null;
            if (!authorizationResult.Succeeded) throw new ForbiddenException("Gebruiker heeft geen toegang", "De gebruiker heeft geen toegang tot dit dossier");

            ViewDossierDTO dossierDTO = new ViewDossierDTO
            {
                Id = dossierModel.Id,
                Title = dossierModel.Title,
                Description = dossierModel.Description,
                DateCreated = dossierModel.DateCreated,
                Documents = dossierModel.Documents.Select(documentModel => new ViewDocumentDTO
                {
                    Id = documentModel.Id,
                    Title = documentModel.Title,
                    Extensions = documentModel.Extensions,
                    DateUploaded = documentModel.DateUploaded,
                }).ToList(),
                InvitedUsers = dossierModel.InvitedUsers.Select(invitedUser => new InvitedUserDTO
                {
                    UserId = invitedUser.UserId,
                    UserName = _userManager.FindByIdAsync(invitedUser.UserId.ToString()).Result.UserName
                }).ToList()
            };

            return dossierDTO;
        }

        public async Task<List<OverviewDossierDTO>> GetAllDossiersAsync(ClaimsPrincipal user)
        {
            string? userId = user.Claims.FirstOrDefault(claim => claim.Type == "userId")?.Value;
            List<DossierModel> dossierModels = await _dossierRepository.GetAllDossiersAsync();

            List<OverviewDossierDTO> overviewDossierDTO = dossierModels
                .Where(dossierModel => dossierModel.UserId == Guid.Parse(userId) || dossierModel.InvitedUsers.Any(invitedUser => invitedUser.UserId == Guid.Parse(userId)))
                .Select(dossierModel => new OverviewDossierDTO
                {
                    Id = dossierModel.Id,
                    Title = dossierModel.Title
                })
                .ToList();

            return overviewDossierDTO;
        }
        public async Task<ViewDossierDTO> CreateDossierAsync(ClaimsPrincipal user, CreateDossierDTO createDossierDTO)
        {
            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(user, createDossierDTO, "DossierCreate");

            if (!authorizationResult.Succeeded) throw new ForbiddenException("Gebruiker heeft geen toegang", "De gebruiker heeft geen toegang tot dit dossier");

            string? userId = user.Claims.FirstOrDefault(claim => claim.Type == "userId")?.Value;
            DossierModel dossierModel = new DossierModel
            {
                Title = createDossierDTO.Title,
                UserId = Guid.Parse(userId),
                Description = createDossierDTO.Description
            };

            DossierModel createdDossierModel = await _dossierRepository.CreateDossierAsync(dossierModel);

            ViewDossierDTO createdDossierDTO = new ViewDossierDTO
            {
                Id = createdDossierModel.Id,
                Title = createdDossierModel.Title,
                Description = createdDossierModel.Description,
                DateCreated = createdDossierModel.DateCreated,
                Documents = dossierModel.Documents.Select(documentModel => new ViewDocumentDTO
                {
                    Id = documentModel.Id,
                    Title = documentModel.Title,
                    Extensions = documentModel.Extensions,
                    DateUploaded = documentModel.DateUploaded
                }).ToList()
            };

            return createdDossierDTO;
        }

        public async Task InviteUserToDossierAsync(ClaimsPrincipal user, Guid dossierId, Guid targetedUserId)
        {
            DossierModel? dossierModel = await _dossierRepository.GetDossierAsync(dossierId);

            if (dossierModel == null) throw new NotFoundForeignKey("Dossier niet gevonden", "Er is geen dossier gevonden met het opgegeven id");

            AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(user, dossierModel, "DossierInvite");

            if (!authorizationResult.Succeeded) throw new ForbiddenException("Gebruiker heeft geen toegang", "De gebruiker heeft geen rechten om mensen uit te nodigen op dit dossier");

            DossierInvitedModel dossierInvitedModel = new DossierInvitedModel
            {
                DossierId = dossierId,
                UserId = targetedUserId
            };

            await _dossierRepository.InviteUserToDossierAsync(dossierInvitedModel);
        }
    }
}
