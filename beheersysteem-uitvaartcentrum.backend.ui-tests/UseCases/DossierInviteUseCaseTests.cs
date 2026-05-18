using Microsoft.Playwright;
using PlaywrightTests;
using NUnit.Framework;

namespace beheersysteem_uitvaartcentrum.backend.ui_tests.UseCases
{
    [TestFixture]
    public class DossierInviteUseCaseTests : BasePageTest
    {
        [Test]
        public async Task DossierInviteUseCase_InviteExternalUser()
        {
            await Page.GotoAsync("https://localhost:5173/login");

            await Page.FillAsync("input[name='email']", "jan.janssen@uitvaart.nl");
            await Page.FillAsync("input[name='password']", "jemoeteenlangwachtwoordhebben");
            await Page.ClickAsync("button[type='submit']");

            var dossierCard = Page.GetByRole(AriaRole.Link, new()
            {
                Name = "Dossier Zonder Extra's"
            });

            await Expect(dossierCard).ToBeVisibleAsync();
            await dossierCard.ClickAsync();

            var invitedUsersBefore = Page.Locator("[aria-label^='Toegang voor:']");
            var countBefore = await invitedUsersBefore.CountAsync();

            await Page.GetByLabel("Nieuwe gebruiker uitnodigen voor dit dossier").ClickAsync();

            var dialog = Page.GetByRole(AriaRole.Dialog);
            await Expect(dialog).ToBeVisibleAsync();

            var firstUser = Page.Locator("[data-testid^='user-radio-']").Last;
            await Expect(firstUser).ToBeVisibleAsync();
            await firstUser.CheckAsync();

            await Page.GetByLabel("confirm-user-invite-to-dossier").ClickAsync();

            await Expect(Page.GetByLabel("confirm-user-invite-to-dossier")).Not.ToBeVisibleAsync();

            var invitedUsersAfter = Page.Locator("[aria-label^='Toegang voor:']");

            await Expect(invitedUsersAfter).ToHaveCountAsync(countBefore + 1);
        }
    }
}