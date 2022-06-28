using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ActionsHomework
{
    public class LabirintSeleniumTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private Actions action;
        
        // Селекторы
        // var cookiePolicyAgree = By.ClassName("cookie-policy__button");
        //
        // var cookiePolicyAgreeBlock = By.XPath("//div[@class='cookie-policy']");
        //
        // var booksMenu = By.XPath("//a[@class='b-header-b-menu-e-text']");
        //
        // var allBooks = By.XPath("//a[contains(@class, 'b-menu-list-title')][contains(@class, 'b-menu-list-title-first')][@href='/books/']");
        //
        // var addBookInCart = By.XPath("(//a[contains(@class, 'buy-link')])[1]");
        //
        // var issueOrder = By.XPath("//a[contains(@class, 'btn-more')][contains(@class, 'buy-link')]");
        //
        // var beginOrder = By.XPath("//*[@id='cart-total-default']//button");
        //
        // var delivery = By.XPath("//button[contains(@class, 'delivery__profiles-change-btn')]");
        //
        // var deliveryAddressInput = By.Id("deliveryAddress");
        //
        // var loader = By.XPath("//div[@class='loading']"); "//*[contains(@src, 'spinner')]"
        //
        // var chooseCourierDelivery = By.XPath("//div[contains(@class, 'delivery-type-items')]//li[2]");
        //
        // var closeButton = By.XPath("//*[@class='delivery--button-block--close']");
        //
        // var modalContainer = By.XPath("//*[@class='modal-container']");
        
        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--incognito");
            options.AddArgument("--start-maximized");
            
            driver = new ChromeDriver(options);
            action = new Actions(driver);
            
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        }

        [Test]
        public void EndToEndTest()
        {
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            
            // Переходим на сайт
            driver.Navigate().GoToUrl("https://www.labirint.ru/");
            
            // Кликнуть по кнопке для принятия политики Cookie
            var cookiePolicyAgree = driver.FindElement(By.ClassName("cookie-policy__button"));
            cookiePolicyAgree.Click();
            
            // В шапке навести на ссылку “Книги”
            var booksMenu = driver.FindElement(By.XPath("//a[@class='b-header-b-menu-e-text']"));
            action.MoveToElement(booksMenu).Build().Perform();
            
            
            // Дождаться показа “Все книги”
            var allBooks = driver.FindElement(By.XPath(
                "//a[contains(@class, 'b-menu-list-title')][contains(@class, 'b-menu-list-title-first')][@href='/books/']"));
            var allBooksLocator = By.XPath("//a[contains(@class, 'b-menu-list-title')][contains(@class, 'b-menu-list-title-first')][@href='/books/']");
            wait.Until(ExpectedConditions.ElementIsVisible(allBooksLocator));
            
            // Кликнуть в раскрывшемся списке по ссылке “Все книги”
            allBooks.Click();
            
            // (*) Проверить, что перешли на страницу с URL = https://www.labirint.ru/books/
            Assert.IsTrue(driver.Url.Contains("https://www.labirint.ru/books/"), "Не перешли на страницу https://www.labirint.ru/books/ !");
            
            // Кликнуть по кнопке “В корзину” у первой книги на странице
            var addBookInCart = driver.FindElement(By.XPath("(//a[contains(@class, 'buy-link')])[1]"));
            addBookInCart.Click();
            
            // Кликнуть по кнопке “Оформить” у первой книги на странице
            var issueOrder = driver.FindElement(By.XPath("//a[contains(@class, 'btn-more')][contains(@class, 'buy-link')]"));
            issueOrder.Click();
            
            // На открывшейся странице кликнуть по кнопке “Начать оформление”
            var beginOrder = driver.FindElement(By.XPath("//*[@id='cart-total-default']//button"));
            beginOrder.Click();
            
            // На открывшейся странице кликнуть по галочке рядом с “Курьерская доставка”
            var delivery = driver.FindElement(By.XPath("//button[contains(@class, 'delivery__profiles-change-btn')]"));
            delivery.Click();
            
            var chooseCourierDelivery = driver.FindElement(By.XPath("//div[contains(@class, 'delivery-type-items')]//li[2]"));
            chooseCourierDelivery.Click();

            // В открывшемся лайтбоксе в “Населенный пункт” ввести некорректный город
            var deliveryAddressInput = driver.FindElement(By.Id("deliveryAddress"));
            deliveryAddressInput.Click();
            deliveryAddressInput.Clear();
            deliveryAddressInput.SendKeys("ывюбвабвьава");
            
            // Убрать фокус с поля, например, кликаем Tab
            deliveryAddressInput.SendKeys(Keys.Tab);
            
            // Проверить, что появилась ошибка “Неизвестный город”
            var inputError = driver.FindElement(By.XPath("//div[contains(@class, 'error-informer')]"));
            Assert.IsTrue(inputError.Displayed, "Не отображается сообщение об ошибке ввода!");

            // Очищаем поле ввода “Населенный пункт” и вводим город “Екатеринбург”
            deliveryAddressInput.Click();
            deliveryAddressInput.Clear();
            deliveryAddressInput.SendKeys("Екатеринбург, Радищева, 31");
            deliveryAddressInput.SendKeys(Keys.Enter);


            // var loader = By.XPath("//div[@class='loading']");
            // wait.Until(ExpectedConditions.ElementIsVisible(loader));
            wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("(//div[@class='delivery--courier-delivery']//li[contains(@class, 'do--list-item')])[1]")));
            var deliveryService = driver.FindElement(By.XPath("(//div[@class='delivery--courier-delivery']//li[contains(@class, 'do--list-item')])[1]"));
            deliveryService.Click();

            var saveButton = driver.FindElement(By.XPath("//div[@class='button-save']"));
            saveButton.Click();

            // Подождать, когда лоадер подсчета даты ближайшей доставки скроется
            var loader = By.XPath("//*[contains(@src, 'spinner')]");
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loader));

            var deliveryInfo = driver.FindElement(By.XPath("//*[@class='delivery__profiles-item-rotate-container']"));
            Assert.IsTrue(deliveryInfo.Displayed, "Информация о выбранной доставке не отображается!");

        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver = null;
        }
        
    }
}