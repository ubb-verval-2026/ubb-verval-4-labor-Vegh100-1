using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DatesAndStuff.Web.Tests;

[TestFixture]
public class BlazeDemoTests
{
    private IWebDriver driver;
    private const string BaseURL = "https://blazedemo.com";

    [SetUp]
    public void SetupTest()
    {
        driver = new ChromeDriver();
    }

    [TearDown]
    public void TeardownTest()
    {
        try
        {
            driver.Quit();
            driver.Dispose();
        }
        catch (Exception)
        {
            // Ignore errors
        }
    }

    [Test]
    public void Flights_MexicoCityToDublin_ShouldHaveAtLeastThreeFlights()
    {
        // Arrange
        driver.Navigate().GoToUrl(BaseURL);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));

        var fromSelect = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("fromPort")));
        var fromDropdown = new SelectElement(fromSelect);
        fromDropdown.SelectByValue("Mexico City");

        var toSelect = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("toPort")));
        var toDropdown = new SelectElement(toSelect);
        toDropdown.SelectByValue("Dublin");

        // Act
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit']"))).Click();

        // Assert
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table.table tbody tr")));
        var flightRows = driver.FindElements(By.CssSelector("table.table tbody tr"));
        flightRows.Count.Should().BeGreaterThanOrEqualTo(3,
            because: "there should be at least 3 flights from Mexico City to Dublin");
    }
}
