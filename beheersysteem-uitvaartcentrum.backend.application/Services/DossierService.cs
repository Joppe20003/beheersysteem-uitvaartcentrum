using beheersysteem_uitvaartcentrum.backend.application.DTOs.Document;
using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using beheersysteem_uitvaartcentrum.backend.domain.Models;

namespace beheersysteem_uitvaartcentrum.backend.application.Services
{
    public class DossierService : IDossierService
    {
        private readonly IDossierRepository _dossierRepository;

        public DossierService(IDossierRepository dossierRepository)
        {
            _dossierRepository = dossierRepository;
        }
        public async Task<ViewDossierDTO?> GetAsync(Guid id)
        {
            DossierModel? dossierModel = await _dossierRepository.getAsync(id);

            ViewDossierDTO? dossierDTO = new ViewDossierDTO
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
                    DateUploaded = documentModel.DateUploaded
                }).ToList()
            };

            return dossierDTO;
        }

        public async Task<List<OverviewDossierDTO>> GetAllAsync()
        {
            List<DossierModel> dossierModels = await _dossierRepository.getAllAsync();

            List<OverviewDossierDTO> overviewDossierDTO = dossierModels.Select(dossierModel => new OverviewDossierDTO
            {
                Id = dossierModel.Id,
                Title = dossierModel.Title
            }).ToList();

            return overviewDossierDTO;
        }
        public async Task<ViewDossierDTO> CreateAsync(CreateDossierDTO dto)
        {
            DossierModel dossierModel = new DossierModel
            {
                Title = dto.Title,
                Description = dto.Description
            };

            DossierModel createdDossierModel = await _dossierRepository.createAsync(dossierModel);

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
    }
}
