using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using beheersysteem_uitvaartcentrum.backend.application.Exceptions;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Repositories;
using beheersysteem_uitvaartcentrum.backend.application.Services;
using beheersysteem_uitvaartcentrum.backend.domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
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

        // ──────────────────────────────────────────────────────────────────────
        // CreateDossierAsync
        // ──────────────────────────────────────────────────────────────────────

        [Test]
        public async Task CreateDossierAsync_HappyPath_ReturnsDossierDTO()
        {
            // Arrange
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

            // Act
            var result = await _dossierService.CreateDossierAsync(user, dto);

            // Assert
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

            // Act and assert
            var exception = Assert.ThrowsAsync<ForbiddenException>(async () => await _dossierService.CreateDossierAsync(user, dto));

            Assert.That(exception.Message, Is.EqualTo("De gebruiker heeft geen rechten om dossier aan te maken"));

            _dossierRepositoryMock.Verify(repo => repo.CreateDossierAsync(It.IsAny<DossierModel>()), Times.Never);
        }

        // ──────────────────────────────────────────────────────────────────────
        // GetAllDossiersAsync
        // ──────────────────────────────────────────────────────────────────────

        [Test]
        public async Task OverviewDossierAsync_HappyPath_ReturnsOnlyDossiersWhereUserIsOwner()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var otherUserId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userId", userId) }, "mock"));

            var dossiers = new List<DossierModel>
            {
                new DossierModel { Id = Guid.NewGuid(), Title = "Eigen dossier", UserId = Guid.Parse(userId), InvitedUsers = new List<DossierInvitedModel>() },
                new DossierModel { Id = Guid.NewGuid(), Title = "Ander dossier", UserId = Guid.Parse(otherUserId), InvitedUsers = new List<DossierInvitedModel>() }
            };

            // Set up
            _dossierRepositoryMock.Setup(repo => repo.GetAllDossiersAsync()).ReturnsAsync(dossiers);

            // Act
            var result = await _dossierService.GetAllDossiersAsync(user);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Title, Is.EqualTo("Eigen dossier"));
        }

        [Test]
        public async Task OverviewDossierAsync_HappyPath_ReturnsOnlyDossiersWhereUserIsInvited()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var otherUserId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userId", userId) }, "mock"));

            var dossiers = new List<DossierModel>
            {
                new DossierModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Uitgenodigd dossier",
                    UserId = Guid.Parse(otherUserId),
                    InvitedUsers = new List<DossierInvitedModel> { new DossierInvitedModel { UserId = userId } }
                },
                new DossierModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Niet uitgenodigd dossier",
                    UserId = Guid.Parse(otherUserId),
                    InvitedUsers = new List<DossierInvitedModel>()
                }
            };

            // Set up
            _dossierRepositoryMock.Setup(repo => repo.GetAllDossiersAsync()).ReturnsAsync(dossiers);

            // Act
            var result = await _dossierService.GetAllDossiersAsync(user);

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.First().Title, Is.EqualTo("Uitgenodigd dossier"));
        }

        // ──────────────────────────────────────────────────────────────────────
        // GetDossierAsync
        // ──────────────────────────────────────────────────────────────────────

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

            // Act
            var result = await _dossierService.GetDossierAsync(user, dossierModel.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(dossierModel.Id));
            Assert.That(result.Title, Is.EqualTo(dossierModel.Title));
            Assert.That(result.Description, Is.EqualTo(dossierModel.Description));
            Assert.That(result.Documents, Is.Empty);
            Assert.That(result.InvitedUsers, Is.Empty);
        }

        [Test]
        public async Task ViewDossierAsync_DossierNotFound_ReturnsNull()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userId", Guid.NewGuid().ToString()) }, "mock"));
            var dossierId = Guid.NewGuid();

            // Set up
            _dossierRepositoryMock.Setup(repo => repo.GetDossierAsync(dossierId)).ReturnsAsync((DossierModel)null);
            _authorizationServiceMock.Setup(auth => auth.AuthorizeAsync(user, (DossierModel)null, "DossierAccess")).ReturnsAsync(AuthorizationResult.Failed());

            // Act
            var result = await _dossierService.GetDossierAsync(user, dossierId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task ViewDossierAsync_AuthorizationFails_ThrowsForbiddenException()
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

            // Act and assert
            var exception = Assert.ThrowsAsync<ForbiddenException>(async () => await _dossierService.GetDossierAsync(user, dossierModel.Id));

            Assert.That(exception.Message, Is.EqualTo("De gebruiker heeft geen toegang tot dit dossier"));
        }

        // ──────────────────────────────────────────────────────────────────────
        // InviteUserToDossierAsync
        // ──────────────────────────────────────────────────────────────────────

        [Test]
        public async Task InviteUserToDossier_HappyPath_InvitesUserToDossier()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var targetedUserId = Guid.NewGuid();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userId", userId) }, "mock"));

            var dossierModel = new DossierModel
            {
                Id = Guid.NewGuid(),
                Title = "Test Dossier",
                UserId = Guid.Parse(userId),
                InvitedUsers = new List<DossierInvitedModel>()
            };

            // Set up
            _dossierRepositoryMock.Setup(repo => repo.GetDossierAsync(dossierModel.Id)).ReturnsAsync(dossierModel);
            _authorizationServiceMock.Setup(auth => auth.AuthorizeAsync(user, dossierModel, "DossierInvite")).ReturnsAsync(AuthorizationResult.Success());
            _dossierRepositoryMock.Setup(repo => repo.InviteUserToDossierAsync(It.IsAny<DossierInvitedModel>())).Returns(Task.CompletedTask);

            // Act
            await _dossierService.InviteUserToDossierAsync(user, dossierModel.Id, targetedUserId);

            // Assert
            _dossierRepositoryMock.Verify(repo => repo.InviteUserToDossierAsync(It.Is<DossierInvitedModel>(d =>
                d.DossierId == dossierModel.Id &&
                d.UserId == targetedUserId.ToString()
            )), Times.Once);
        }

        [Test]
        public async Task InviteUserToDossier_UserAlreadyInvited_ThrowsAlreadyExistsException()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var targetedUserId = Guid.NewGuid();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userId", userId) }, "mock"));

            var dossierModel = new DossierModel
            {
                Id = Guid.NewGuid(),
                Title = "Test Dossier",
                UserId = Guid.Parse(userId),
                InvitedUsers = new List<DossierInvitedModel>
                {
                    new DossierInvitedModel { UserId = targetedUserId.ToString() }
                }
            };

            // Set up
            _dossierRepositoryMock.Setup(repo => repo.GetDossierAsync(dossierModel.Id)).ReturnsAsync(dossierModel);
            _authorizationServiceMock.Setup(auth => auth.AuthorizeAsync(user, dossierModel, "DossierInvite")).ReturnsAsync(AuthorizationResult.Success());

            // Act and assert
            var exception = Assert.ThrowsAsync<AlreadyExistsException>(async () => await _dossierService.InviteUserToDossierAsync(user, dossierModel.Id, targetedUserId));

            Assert.That(exception.Message, Is.EqualTo("Gebruiker is al uitgenodigd voor dit dossier."));

            _dossierRepositoryMock.Verify(repo => repo.InviteUserToDossierAsync(It.IsAny<DossierInvitedModel>()), Times.Never);
        }
    }
}