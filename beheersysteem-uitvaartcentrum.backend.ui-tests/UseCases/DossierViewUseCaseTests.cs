using Microsoft.Playwright;
using PlaywrightTests;

namespace beheersysteem_uitvaartcentrum.backend.ui_tests.UseCases
{
    [TestFixture]
    public class DossierViewUseCaseTests : BasePageTest
    {
        [Test]
        public async Task DossierViewUseCase_GetDossierDetails()
        {
            await Page.GotoAsync("https://localhost:5173/login");
            await Page.FillAsync("input[name='email']", "jan.janssen@uitvaart.nl");
            await Page.FillAsync("input[name='password']", "jemoeteenlangwachtwoordhebben");
            await Page.ClickAsync("button[type='submit']");

            var dossierCard = Page.GetByRole(AriaRole.Link, new()
            {
                Name = "Dossier Zonder Extra's",
                Exact = false
            });

            await dossierCard.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await dossierCard.ClickAsync();

            var dossierTitle = Page.GetByLabel("dossier-title-field");
            var dossierDescription = Page.GetByLabel("dossier-description-field");
            var dossierCreatedDate = Page.GetByLabel("dossier-created-date-field");

            await Expect(dossierTitle).ToHaveTextAsync("Dossier Zonder Extra's");
            await Expect(dossierDescription).ToHaveTextAsync("Eigendom van Jan, geen genodigden.");
            await Expect(dossierCreatedDate).ToHaveTextAsync("21:16, 16-5-2026");
        }
    }
}