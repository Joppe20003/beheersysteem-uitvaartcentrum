using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using beheersysteem_uitvaartcentrum.backend.application.Exceptions;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using beheersysteem_uitvaartcentrum.backend.application.Services;
using beheersysteem_uitvaartcentrum.backend.domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;

namespace beheersysteem_uitvaartcentrum.backend.unit_tests.Services
{
    [TestFixture]
    public class DossierServiceTests
    {
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private Mock<IDossierRepository> _dossierRepositoryMock;
        private Mock<IAuthorizationService> _authorizationServiceMock;
        private DossierService _dossierService;

        [SetUp]
        public void Setup()
        {
            var store = new Mock<IUserStore<IdentityUser>>();

            _userManagerMock = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            _dossierRepositoryMock = new Mock<IDossierRepository>();
            _authorizationServiceMock = new Mock<IAuthorizationService>();
            _dossierService = new DossierService(_userManagerMock.Object, _dossierRepositoryMock.Object, _authorizationServiceMock.Object);
        }

        [Test]
        public async Task CreateDossierAsync_HappyPath_ReturnsDossierDTO()
        {
            // Arange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userId", Guid.NewGuid().ToString()) }, "mock"));
            var dto = new CreateDossierDTO
            {
                Title = "Test Dossier",
                Description = "This is a test dossier."
            };
            var createdDossier = new DossierModel
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                UserId = Guid.Parse(user.Claims.First(c => c.Type == "userId").Value),
                DateCreated = DateTime.UtcNow
            };

            // Set up
            _authorizationServiceMock.Setup(auth => auth.AuthorizeAsync(user, dto, "DossierCreate")).ReturnsAsync(AuthorizationResult.Success());
            _dossierRepositoryMock.Setup(repo => repo.CreateDossierAsync(It.IsAny<DossierModel>())).ReturnsAsync(createdDossier);

            // Assert and act
            var result = await _dossierService.CreateDossierAsync(user, dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Title, Is.EqualTo(dto.Title));
            Assert.That(result.Description, Is.EqualTo(dto.Description));
            Assert.That(result.UserId, Is.EqualTo(Guid.Parse(user.Claims.First(c => c.Type == "userId").Value)));

            _dossierRepositoryMock.Verify(repo => repo.CreateDossierAsync(It.Is<DossierModel>(d =>
                d.Title == dto.Title &&
                d.Description == dto.Description &&
                d.UserId == Guid.Parse(user.Claims.First(c => c.Type == "userId").Value)
            )), Times.Once);
        }

        [Test]
        public async Task CreateDossierAsync_AuthorizationFails_ThrowsForbiddenException()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userId", Guid.NewGuid().ToString()) }, "mock"));
            var dto = new CreateDossierDTO
            {
                Title = "Test Dossier",
                Description = "This is a test dossier."
            };

            // Set up
            _authorizationServiceMock.Setup(auth => auth.AuthorizeAsync(user, dto, "DossierCreate")).ReturnsAsync(AuthorizationResult.Failed());

            // Assert and act
            var exception = Assert.ThrowsAsync<ForbiddenException>(async () => await _dossierService.CreateDossierAsync(user, dto));

            Assert.That(exception.Message, Is.EqualTo("De gebruiker heeft geen rechten om dossier aan te maken"));

            _dossierRepositoryMock.Verify(repo => repo.CreateDossierAsync(It.IsAny<DossierModel>()), Times.Never);
        }

        [Test]
        public async Task OverviewDossierAsync_HappyPath_ReturnsListOfDossiers()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userId", Guid.NewGuid().ToString()) }, "mock"));
            var dossiers = new List<DossierModel>
            {
                new DossierModel { Id = Guid.NewGuid(), Title = "Dossier 1", UserId = Guid.Parse(user.Claims.First(c => c.Type == "userId").Value) },
                new DossierModel { Id = Guid.NewGuid(), Title = "Dossier 2", UserId = Guid.Parse(user.Claims.First(c => c.Type == "userId").Value) }
            };
            
            // Set up
            _dossierRepositoryMock.Setup(repo => repo.GetAllDossiersAsync()).ReturnsAsync(dossiers);

            // Assert and act
            var result = await _dossierService.GetAllDossiersAsync(user);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(dossiers.Count));
            Assert.That(result.All(d => dossiers.Any(dm => dm.Id == d.Id)), Is.True);
        }

        [Test]
        public async Task ViewDossierAsync_HappyPath_ReturnsViewDossierDTO()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userId", Guid.NewGuid().ToString()) }, "mock"));
            var dossierModel = new DossierModel
            {
                Id = Guid.NewGuid(),
                Title = "Test Dossier",
                Description = "Test omschrijving",
                UserId = Guid.Parse(user.Claims.First(c => c.Type == "userId").Value),
                DateCreated = DateTime.UtcNow,
                Documents = new List<DocumentModel>(),
                InvitedUsers = new List<DossierInvitedModel>()
            };

            // Set up
            _dossierRepositoryMock.Setup(repo => repo.GetDossierAsync(dossierModel.Id)).ReturnsAsync(dossierModel);
            _authorizationServiceMock.Setup(auth => auth.AuthorizeAsync(user, dossierModel, "DossierAccess")).ReturnsAsync(AuthorizationResult.Success());

            // Assert and act
            var result = await _dossierService.GetDossierAsync(user, dossierModel.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(dossierModel.Id));
            Assert.That(result.Title, Is.EqualTo(dossierModel.Title));
            Assert.That(result.Description, Is.EqualTo(dossierModel.Description));
            Assert.That(result.Documents, Is.Empty);
            Assert.That(result.InvitedUsers, Is.Empty);
        }

        [Test]
        public async Task ViewDossierAsync_AuthorizationDossierAccessByUserIdFails_ThrowsForbiddenException()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userId", Guid.NewGuid().ToString()) }, "mock"));
            var dossierModel = new DossierModel
            {
                Id = Guid.NewGuid(),
                Title = "Test Dossier",
                Description = "Test omschrijving",
                UserId = Guid.NewGuid(),
                DateCreated = DateTime.UtcNow,
                Documents = new List<DocumentModel>(),
                InvitedUsers = new List<DossierInvitedModel>()
            };

            // Set up
            _dossierRepositoryMock.Setup(repo => repo.GetDossierAsync(dossierModel.Id)).ReturnsAsync(dossierModel);
            _authorizationServiceMock.Setup(auth => auth.AuthorizeAsync(user, dossierModel, "DossierAccess")).ReturnsAsync(AuthorizationResult.Failed());

            // Assert and act
            var exception = Assert.ThrowsAsync<ForbiddenException>(async () => await _dossierService.GetDossierAsync(user, dossierModel.Id));

            Assert.That(exception.Message, Is.EqualTo("De gebruiker heeft geen toegang tot dit dossier"));
        }

        [Test]
        public async Task ViewDossierAsync_AuthorizationDossierAccessByInvitedUserIdFails_ThrowsForbiddenException()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userId", Guid.NewGuid().ToString()) }, "mock"));
            var dossierModel = new DossierModel
            {
                Id = Guid.NewGuid(),
                Title = "Test Dossier",
                Description = "Test omschrijving",
                UserId = Guid.NewGuid(),
                DateCreated = DateTime.UtcNow,
                Documents = new List<DocumentModel>(),
                InvitedUsers = new List<DossierInvitedModel>()
            };

            // Set up
            _dossierRepositoryMock.Setup(repo => repo.GetDossierAsync(dossierModel.Id)).ReturnsAsync(dossierModel);
            _authorizationServiceMock.Setup(auth => auth.AuthorizeAsync(user, dossierModel, "DossierAccess")).ReturnsAsync(AuthorizationResult.Failed());

            // Assert and act
            var exception = Assert.ThrowsAsync<ForbiddenException>(async () => await _dossierService.GetDossierAsync(user, dossierModel.Id));

            Assert.That(exception.Message, Is.EqualTo("De gebruiker heeft geen toegang tot dit dossier"));
        }
    }
}