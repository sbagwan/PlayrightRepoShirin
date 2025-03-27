
using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using Microsoft.Playwright;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LambdatestPlaywrightTests
{
    
   
        [TestFixture]
        [AllureNUnit]
        public class PlaywrightTests
        {
            private IPage _page;
            private IBrowser _browser;
            private IBrowserContext _context;
            private IPlaywright _playwright;
            private TestLocators _testLocator;

            [SetUp]
            [AllureBefore("Setup session")]
            public async Task Setup()
            {
                string user, accessKey;
                user = "shirin_bagwan";
                accessKey = "LT_7O30hK5HeoGhWj0z1ZDiNVsEfFFov0yO38O2LF60ajSRMyP";

                Dictionary<string, object> capabilities = new Dictionary<string, object>();
                Dictionary<string, string> ltOptions1 = new Dictionary<string, string>();

            Dictionary<string, object> ltOptions = new Dictionary<string, object>
            {
               
                { "visual", true },
                { "video", true },
                { "terminal", true },
                { "devicelog", true },
                { "build", "BuildprojectPlayright_1" },
                { "console", true },
                { "platformName", "Windows 10" },
                { "network", true },
                { "project", "Playwrighttest" },
                 { "name", TestContext.CurrentContext.Test.ClassName + "-" + TestContext.CurrentContext.Test.MethodName  },
                { "w3c", true }
            };


            capabilities.Add("user",user);
            capabilities.Add("accessKey", accessKey);

            capabilities.Add("browserName", "chrome");
                capabilities.Add("browserVersion", "latest");
                capabilities.Add("LT:Options", ltOptions);
                string capabilitiesJson = JsonConvert.SerializeObject(capabilities);
                string cdpUrl = "wss://cdp.lambdatest.com/playwright?capabilities=" + Uri.EscapeDataString(capabilitiesJson);
                _playwright = await Playwright.CreateAsync();
                _browser = await _playwright.Chromium.ConnectAsync(cdpUrl);
                _context = await _browser.NewContextAsync(new());
                _page = await _context.NewPageAsync();
                _testLocator = new TestLocators(_page);
                _context = await _browser.NewContextAsync();
                await _context.Tracing.StartAsync(new TracingStartOptions
                {
                    Title = "Test Trace",
                    Screenshots = true,
                    Snapshots = true,
                    Sources = true
                });

            }

            [Test]
            [AllureName("Scenario is for Simple form Demo")]
            public async Task TestScenario1()
            {
                await _page.GotoAsync("https://www.lambdatest.com/selenium-playground/");
                await _testLocator.ClickAsync(_testLocator.SimpleFrmDemo);
                await _testLocator.EnterTextAsync(_testLocator.SimplefrmIp, "Welcome to LambdaTest");
                await _testLocator.ClickAsync(_testLocator.GetCheckedValue);
                Assert.That(_page.Url, Does.Contain("simple-form-demo"));
                Assert.That(await _testLocator.SampleMsg.InnerTextAsync(), Is.EqualTo("Welcome to LambdaTest"));
                var actualMeesageText = await _testLocator.SampleMsg.InnerTextAsync();
                var expMessageText = "Welcome to LambdaTest";
                Assert.That(actualMeesageText.Equals(expMessageText));
                if (await _testLocator.SampleMsg.InnerTextAsync() == "Welcome to LambdaTest")
                {
                    await SetTestStatus("passed", "Messaged matched", _page);
                }
                else
                {
                    await SetTestStatus("failed", "Messaged not matched", _page);
                }

            }


            [Test]
            [AllureName("Scenario is for Drag and Drop")]
            public async Task TestScenario2()
            {
                await _page.GotoAsync("https://www.lambdatest.com/selenium-playground/");
                await _testLocator.ClickAsync(_testLocator.DragAndDrop);
                await _page.EvaluateAsync(@"() => {
                document.body.style.zoom = '0.50'; 
            }");
                var dragger = _testLocator.Slider;
                var box = await dragger.BoundingBoxAsync();

                if (box != null)
                {
                    await _page.Mouse.MoveAsync(box.X + box.Width / 2, box.Y + box.Height / 2);
                    await _page.Mouse.DownAsync();
                    await _page.Mouse.MoveAsync(box.X + box.Width / 2 + 107, box.Y + box.Height / 2);
                    await _page.Mouse.UpAsync();
                }

                var textContent = await _testLocator.SliderValue.TextContentAsync();
                Assert.That(textContent, Is.EqualTo("95"));
                if (textContent == "95")
                {
                    await SetTestStatus("passed", "Slidder value matched", _page);
                }
                else
                {
                    await SetTestStatus("failed", "Slidder value not matched", _page);
                }
            }

            [Test]
            [AllureName("Scenario is for Input Form")]
            public async Task TestScenario3()
            {
                await _page.GotoAsync("https://www.lambdatest.com/selenium-playground/");
                await _page.SetViewportSizeAsync(1920, 1080);
                await _testLocator.ClickAsync(_testLocator.InputForm);
                await _testLocator.ClickAsync(_testLocator.SubmitForm);
                var validationMessage = await _page.EvaluateAsync<string>("document.activeElement.validationMessage");
                Assert.That(validationMessage, Is.EqualTo("Please fill out this field."));
                await _testLocator.EnterTextAsync(_testLocator.Name, "Lambda");
                await _testLocator.EnterTextAsync(_testLocator.Email, "lambda@email.com");
                await _testLocator.EnterTextAsync(_testLocator.Password, "PaSsWoRd");
                await _testLocator.EnterTextAsync(_testLocator.Company, "LambdaTest");
                await _testLocator.EnterTextAsync(_testLocator.Website, "https://www.lambdatest.com");
                await _testLocator.Country.SelectOptionAsync(new SelectOptionValue { Label = "United States" });
                await _testLocator.EnterTextAsync(_testLocator.City, "City");
                await _testLocator.EnterTextAsync(_testLocator.Address1, "Address1");
                await _testLocator.EnterTextAsync(_testLocator.Address2, "Address2");
                await _testLocator.EnterTextAsync(_testLocator.State, "State");
                await _testLocator.EnterTextAsync(_testLocator.Zip, "12345");
                await _testLocator.ClickAsync(_testLocator.SubmitForm);

                Assert.Multiple(async () =>
                {
                    Assert.That(await _testLocator.SuccessMsg.IsVisibleAsync(), Is.True);
                    Assert.That(await _testLocator.SuccessMsg.InnerTextAsync(), Is.EqualTo("Thanks for contacting us, we will get back to you shortly."));
                });

                if (await _testLocator.SuccessMsg.InnerTextAsync() == "Thanks for contacting us, we will get back to you shortly.")
                {
                    await SetTestStatus("passed", "Title matched", _page);
                }
                else
                {
                    await SetTestStatus("failed", "Title not matched", _page);
                }
            }

            [TearDown]
            [AllureAfter("Dispose session")]
            public async Task Close()
            {
                await _context.Tracing.StopAsync(new TracingStopOptions
                {
                    Path = "trace.zip"
                });

                await _browser.CloseAsync();
                _playwright.Dispose();
            }
            public static async Task SetTestStatus(string status, string remark, IPage page)
            {
                await page.EvaluateAsync("_ => {}", "lambdatest_action: {\"action\": \"setTestStatus\", \"arguments\": {\"status\":\"" + status + "\", \"remark\": \"" + remark + "\"}}");
            }
        }
    }
