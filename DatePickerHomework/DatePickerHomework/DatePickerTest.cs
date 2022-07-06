using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace DatePickerHomework
{
    public class DatePickerTest
    {
        private IWebDriver driver;
        
        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--incognito");
            options.AddArgument("--start-maximized");
            
            driver = new ChromeDriver(options);
        }

        [Test]
        public void EndToEndTest()
        {
            driver.Navigate().GoToUrl("https://jqueryui.com/datepicker/");

            var frameElement = driver.FindElement(By.TagName("iframe"));
            driver.SwitchTo().Frame(frameElement);

            var date = DateTime.Today.AddDays(8).ToString("dd-MM-yyyy").Replace("-", "/");

            (driver as IJavaScriptExecutor).ExecuteScript($"$('#datepicker').datepicker('setDate', '{date}')");
            
            var calendar = driver.FindElement(By.Id("datepicker"));
            
            var resultDate = calendar.GetAttribute("value");
            
            Assert.AreEqual(date, resultDate);
        }
        
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver = null;
        }
    }
}