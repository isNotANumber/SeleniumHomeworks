using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace LocatorsHomework
{
    public class LabirintSeleniumTests
    {
        private IWebDriver driver;
        private Actions action;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            
            options.AddArguments("--incognito");
            options.AddArgument("--start-maximized");
            
            driver = new ChromeDriver(options);
            action = new Actions(driver);
            
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            
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
            // var beginOrder = By.XPath("//*[@id='cart-total-default']");
            //
            // var delivery = By.XPath("//button[contains(@class, 'delivery__profiles-change-btn')]");
            //
            // var deliveryAddressInput = By.Id("deliveryAddress");
            //
            // var loader = By.XPath("//div[@class='loading']");
            //
            // var chooseCourierDelivery = By.XPath("//div[contains(@class, 'delivery-type-items')]//li[2]");
            //
            // var closeButton = By.XPath("//*[@class='delivery--button-block--close']");
            //
            // var modalContainer = By.XPath("//*[@class='modal-container']");
        }

        [Test]
        public void CookiePolicyAgreeTest()
        {
            driver.Navigate().GoToUrl("https://www.labirint.ru/");
            
            var cookiePolicyAgree = driver.FindElement(By.ClassName("cookie-policy__button"));
            cookiePolicyAgree.Click();
            
            var cookiePolicyAgreeBlock = driver.FindElement(By.XPath("//div[@class='cookie-policy']"));
            
            Assert.IsFalse(cookiePolicyAgreeBlock.Displayed, "Нажали кнопку 'Принять', но блок с куками не исчез!");
        }

        [Test]
        public void BookMenuTest()
        {
            driver.Navigate().GoToUrl("https://www.labirint.ru/");
            
            var booksMenu = driver.FindElement(By.XPath("//a[@class='b-header-b-menu-e-text']"));
            booksMenu.Click();
            
            Assert.IsTrue(driver.Title.Contains("Купить книги"), "Не перешли на страницу с книгами!");
        }

        [Test]
        public void AllBooksTest()
        {
            driver.Navigate().GoToUrl("https://www.labirint.ru/");
            
            var booksMenu = driver.FindElement(By.XPath("//a[@class='b-header-b-menu-e-text']"));
            action.MoveToElement(booksMenu).Perform();
            
            var allBooks = driver.FindElement(By.XPath("//a[@class='b-menu-list-title b-menu-list-title-first'][@href='/books/']"));
            allBooks.Click();
            
            Assert.IsTrue(driver.Title.Contains("Купить книги"), "Не перешли на страницу с книгами!");
        }

        [Test]
        public void AddBookInCartTest()
        {
            driver.Navigate().GoToUrl("https://www.labirint.ru/books/");
            
            var addBookInCart = driver.FindElement(By.XPath("(//a[contains(@class, 'buy-link')])[1]"));
            addBookInCart.Click();

            var basketLink = driver.FindElement((By.LinkText("Оформить заказ")));
            
            Assert.IsTrue(basketLink.Displayed, "Товар не добавился в корзину!");
        }

        [Test]
        public void IssueOrderTest()
        {
            driver.Navigate().GoToUrl("https://www.labirint.ru/books/");
            
            var addBookInCart = driver.FindElement(By.XPath("//a[contains(@class, 'btn-more')][contains(@class, 'buy-link')]"));
            addBookInCart.Click();
            
            var issueOrder = driver.FindElement(By.XPath("//a[@class='btn buy-link btn-primary btn-more']"));
            issueOrder.Click();
            
            Assert.IsTrue(driver.Url.Contains("/cart/"), "Не перешли в корзину!");
        }

        [Test]
        public void BeginOrderTest()
        {
            driver.Navigate().GoToUrl("https://www.labirint.ru/books/");
            
            var addBookInCart = driver.FindElement(By.XPath("//div[@data-action-name='Лучшая покупка дня']//a[@data-position=1]"));
            addBookInCart.Click();
            
            var issueOrder = driver.FindElement(By.XPath("//a[@class='btn buy-link btn-primary btn-more']"));
            issueOrder.Click();
            
            var beginOrder = driver.FindElement(By.XPath("//*[@id='cart-total-default']"));
            beginOrder.Click();

            // Костыль для ожидания загрузки страницы, поскольку не помню как правильно прописывать явный wait
            driver.FindElement(By.ClassName("_title-generic"));

            Assert.IsTrue(driver.Url.Contains("/basket/checkout/"), "Не перешли к оформлению!");
        }

        [Test]
        public void ChooseCourierDeliveryTest()
        {
            driver.Navigate().GoToUrl("https://www.labirint.ru/books/");
            
            var addBookInCart = driver.FindElement(By.XPath("//div[@data-action-name='Лучшая покупка дня']//a[@data-position=1]"));
            addBookInCart.Click();
            
            var issueOrder = driver.FindElement(By.XPath("//a[@class='btn buy-link btn-primary btn-more']"));
            issueOrder.Click();
            
            var beginOrder = driver.FindElement(By.XPath("//div[@id='cart-total-default']//button[@type='submit']"));
            beginOrder.Click();
            
            var delivery = driver.FindElement(By.XPath("//button[contains(@class, 'delivery__profiles-change-btn')]"));
            delivery.Click();

            var placeInput = driver.FindElement(By.Id("deliveryAddress"));
            placeInput.SendKeys("Екатеринбург");
            
            var chooseCourierDelivery = driver.FindElement(By.XPath("//div[contains(@class, 'delivery-type-items')]//li[2]"));
            chooseCourierDelivery.Click();

            // var loader = driver.FindElement(By.XPath("//div[@class='loading']"));

            Assert.IsTrue(driver.FindElement(By.XPath("//div[@class='delivery--courier-delivery']")).Displayed, "Блок с вариантами доставки не отразился!");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver = null;
        }
    }
}