using Microsoft.Playwright;

namespace PlaywrightTests
{
    public class BasePageTest : PageTest
    {
        public override BrowserNewContextOptions ContextOptions()
        {
            return new BrowserNewContextOptions()
            {
                IgnoreHTTPSErrors = true,
                ViewportSize = new()
                {
                    Width = 1920,
                    Height = 1080
                }
            };
        }

    }
}