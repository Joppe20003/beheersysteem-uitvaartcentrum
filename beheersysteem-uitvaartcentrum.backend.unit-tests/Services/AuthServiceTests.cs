using beheersysteem_uitvaartcentrum.backend.application.DTOs.Auth;
using beheersysteem_uitvaartcentrum.backend.application.Exceptions;
using beheersysteem_uitvaartcentrum.backend.application.Interfaces.Services;
using beheersysteem_uitvaartcentrum.backend.application.Services;
using beheersysteem_uitvaartcentrum.backend.domain.Enums;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace beheersysteem_uitvaartcentrum.backend.unit_tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<UserManager<IdentityUser>> _userManagerMock;
        private Mock<ITokenService> _tokenServiceMock;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            var store = new Mock<IUserStore<IdentityUser>>();

            _userManagerMock = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);

            _tokenServiceMock = new Mock<ITokenService>();
            _authService = new AuthService(_userManagerMock.Object, _tokenServiceMock.Object);
        }

        [Test]
        public async Task RegisterAsync_HappyPath_ShouldCreateUserAndAddRole()
        {
            // Arrange
            var dto = new RegisterDTO
            {
                Email = "jan.klaassen@gmail.com",
                Username = "Jan Klaassen",
                Password = "jemoeteenlangwachtwoordhebben",
                Role = RolesRequest.UitvaartOndernemer
            };

            // Set up
            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email)).ReturnsAsync((IdentityUser)null);
            _userManagerMock.Setup(x => x.FindByNameAsync(dto.Username)).ReturnsAsync((IdentityUser)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), dto.Password)).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), dto.Role.ToString())).ReturnsAsync(IdentityResult.Success);

            // Act
            await _authService.RegisterAsync(dto);

            // Assert
            _userManagerMock.Verify(x => x.CreateAsync(It.Is<IdentityUser>(u => u.Email == dto.Email && u.UserName == dto.Username), dto.Password), Times.Once);

            _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), dto.Role.ToString()), Times.Once);
        }

        [Test]
        public async Task RegisterAsync_EmailAlreadyExists_ThrowsAlreadyExistsException()
        {
            // Arrange
            var dto = new RegisterDTO
            {
                Email = "jan.klaassen@gmail.com",
                Username = "Jan Klaassen",
                Password = "jemoeteenlangwachtwoordhebben",
                Role = RolesRequest.UitvaartOndernemer
            };

            // Set up
            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email)).ReturnsAsync(new IdentityUser { Email = dto.Email });

            // Assert and act
            var exception = Assert.ThrowsAsync<AlreadyExistsException>(async () => await _authService.RegisterAsync(dto));

            Assert.That(exception.Message, Is.EqualTo("Kan geen account aanmaken, Email is al in gebruik"));

            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task RegisterAsync_UsernameAlreadyExists_ThrowsAlreadyExistsException()
        {
            // Arrange
            var dto = new RegisterDTO
            {
                Email = "jan.klaassen@gmail.com",
                Username = "Jan Klaassen",
                Password = "jemoeteenlangwachtwoordhebben",
                Role = RolesRequest.UitvaartOndernemer
            };

            // Set up
            _userManagerMock.Setup(x => x.FindByNameAsync(dto.Username)).ReturnsAsync(new IdentityUser { UserName = dto.Username });

            // Assert and act
            var exception = Assert.ThrowsAsync<AlreadyExistsException>(async () => await _authService.RegisterAsync(dto));

            Assert.That(exception.Message, Is.EqualTo("Kan geen account aanmaken, Gebruikersnaam is al in gebruik."));

            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task LoginAsync_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var dto = new LoginDTO
            {
                Email = "jan.klaassen@gmail.com",
                Password = "CorrectWachtwoord123!"
            };

            var user = new IdentityUser { Email = dto.Email, UserName = "Jan Klaassen" };
            var expectedToken = "fake-jwt-token";

            // Mocks instellen
            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, dto.Password)).ReturnsAsync(true);
            _tokenServiceMock.Setup(x => x.GenerateTokenAsync(user)).ReturnsAsync(expectedToken);

            // Assert and act
            var result = await _authService.LoginAsync(dto);

            Assert.That(result, Is.EqualTo(expectedToken));

            _tokenServiceMock.Verify(x => x.GenerateTokenAsync(user), Times.Once);
        }

        [Test]
        public async Task LoginAsync_InvalidPassword_ThrowsForbiddenException()
        {
            // Arrange
            var dto = new LoginDTO
            {
                Email = "jan.klaassen@gmail.com",
                Password = "VerkeerdWachtwoord"
            };

            var user = new IdentityUser { Email = dto.Email };

            _userManagerMock.Setup(x => x.FindByEmailAsync(dto.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, dto.Password)).ReturnsAsync(false); // Wachtwoord is FOUT

            // Assert and act
            var exception = Assert.ThrowsAsync<ForbiddenException>(async () =>await _authService.LoginAsync(dto));

            Assert.That(exception.Message, Is.EqualTo("Email of wachtwoord incorrect"));

            _tokenServiceMock.Verify(x => x.GenerateTokenAsync(It.IsAny<IdentityUser>()), Times.Never);
        }
    }
}