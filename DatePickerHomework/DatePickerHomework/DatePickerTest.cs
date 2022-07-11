using System;
using System.Globalization;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace DatePickerHomework
{
    public class DatePickerTest
    {
        private IWebDriver driver;

        private string FormatDate(DateTime date)
        {
            return date.ToString("MM'/'dd'/'yyyy");
        }

        private string FormatDate(string date)
        {
            var resultDate = DateTime.Parse(date, CultureInfo.InvariantCulture);

            return resultDate.ToString("MM'/'dd'/'yyyy");
        }

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

            var date = FormatDate(DateTime.Today.AddDays(8));

            (driver as IJavaScriptExecutor).ExecuteScript($"$('#datepicker').datepicker('setDate', '{date}')");
            
            // Варианты извлечения значения:
            // var calendar = driver.FindElement(By.Id("datepicker"));
            // var resultDate = FormatDate(calendar.GetAttribute("value"));
            // var resultDate = FormatDate((driver as IJavaScriptExecutor).ExecuteScript("return $('#datepicker').datepicker('getDate')").ToString());
            
            // Финальный вариант:
            var resultDate = FormatDate((driver as IJavaScriptExecutor).ExecuteScript("return $('#datepicker').val();").ToString());
            
            Assert.AreEqual(date, resultDate, "Дата выставлена некорректно!");
        }
        
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver = null;
        }
    }
}