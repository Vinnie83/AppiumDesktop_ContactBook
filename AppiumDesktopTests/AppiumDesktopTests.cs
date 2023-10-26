
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;

namespace AppiumDesktopTests
{
    public class AppiumDesktopTests
    {

        public WindowsDriver<WindowsElement> driver;
        public AppiumOptions options;

        private const string appLocation = @"C:\ContactBook-DesktopClient.NET6\ContactBook-DesktopClient.exe";
        public const string appiumServer = "http://127.0.0.1:4723/wd/hub";
        public const string appServer = "https://contactbook.velio4ka.repl.co/api";
       

        [SetUp] 

        public void SetupApp()
        {
            this.options = new AppiumOptions();
            options.AddAdditionalCapability("app", appLocation);
            this.driver = new WindowsDriver<WindowsElement> (new Uri(appiumServer), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]  

        public void CloseApp()
        {

        }

        [Test]

        public void Test_SearchContact()
        {
            var inputFieldApi = driver.FindElementByAccessibilityId("textBoxApiUrl");
            inputFieldApi.Clear();
            inputFieldApi.SendKeys(appServer);

            var connectButton = driver.FindElementByAccessibilityId("buttonConnect");
            connectButton.Click();

            var activeWindow = driver.WindowHandles[0];
            driver.SwitchTo().Window(activeWindow);

            var inputField = driver.FindElementByAccessibilityId("textBoxSearch");
            inputField.SendKeys("steve");

            var searchField = driver.FindElementByAccessibilityId("buttonSearch");
            searchField.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => {
                return driver.FindElementByAccessibilityId("labelResult").Text.Contains("Contacts found:");
            });

            var foundContactCell = driver.FindElementByAccessibilityId("dataGridViewContacts");

            var columnFirstName = driver.FindElementByXPath("//DataItem[@Name='FirstName Row 0, Not sorted.']");
            Assert.That(columnFirstName.Text, Is.EqualTo("Steve"));

            var columnLastName = driver.FindElementByXPath("//DataItem[@Name='LastName Row 0, Not sorted.']");
            Assert.That(columnLastName.Text, Is.EqualTo("Jobs"));  

           

        }
        
    }
}