using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SeleniumNew3
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Remote;
    using OpenQA.Selenium.Support.UI;

    using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

    [TestClass]
    public class MyFirstTest
    {
        private IWebDriver driver;

        private WebDriverWait wait;

        #region ConstVariables

        private const string AdminLiteCartUrl = "http://localhost:8084/litecart/admin";
        private const string LiteCartUrl = "http://localhost:8084/litecart";
        private const string AdminName = "admin";
        private const string AdminPassword = "admin";

        #endregion
        [SetUp]
        public void Start()
        {
            this.driver = new ChromeDriver();
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            this.wait =new WebDriverWait(this.driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void FirstTest()
        {
            this.driver.Url = "http://google.com";
            this.driver.FindElement(By.Name("q")).SendKeys("webdriver");
            this.driver.FindElement(By.Name("btnK")).Click();
            this.wait.Until(ExpectedConditions.TitleIs("webdriver - Поиск в Google"));
        }

        [Test]
        public void LoginInAdminPanel()
        {
            this.driver.Url = AdminLiteCartUrl;
            this.driver.FindElement(By.Name("username")).Clear();
            this.driver.FindElement(By.Name("password")).Clear();
            this.driver.FindElement(By.Name("username")).SendKeys(AdminName);
            this.driver.FindElement(By.Name("password")).SendKeys(AdminPassword);

            this.driver.FindElement(By.Name("login")).Click();
        }

        [Test]
        public void Zad7_ClickOnAllMenuItem()
        {
            this.LoginInAdminPanel();

            var listFirstLevelMenu = this.driver.FindElements(By.XPath(".//ul[@id='box-apps-menu']/li/a"));
            var firstLevelListItem = listFirstLevelMenu.Select(menuItem => menuItem.GetAttribute("href")).ToList();

            foreach (var firstItem in firstLevelListItem)
            {
                this.driver.FindElement(By.XPath($".//a[@href='{firstItem}']/span")).Click();
                Assert.IsTrue(driver.FindElements(By.XPath(".//h1")).Count > 0);

                var listSecondLevelmenu = this.driver.FindElements(By.XPath(".//li[@id='app-' and @class='selected']/ul[@class='docs']//a"));
                var secondLevelListItem = listSecondLevelmenu.Select(menuItem => menuItem.GetAttribute("href")).ToList();

                foreach (var secondItem in secondLevelListItem)
                {
                    this.driver.FindElement(By.XPath($".//a[@href='{secondItem}']/span")).Click();
                    Assert.IsTrue(driver.FindElements(By.XPath(".//h1")).Count > 0);
                }
            }
            
        }

        [Test]
        public void Zad8_ValidateStiker()
        {
            this.driver.Url = LiteCartUrl;
            var productItems = this.driver.FindElements(By.XPath(".//li[@class='product column shadow hover-light']"));

            foreach (var product in productItems)
            {
                var nameProduct = product.FindElement(By.XPath(".//div[@class='name']")).Text;
                var sticker = product.FindElements(By.XPath(".//div[contains(@class, 'sticker')]"));
                Assert.AreEqual(1, sticker.Count, $"Sticker for product {nameProduct} != 1");
            }
        }

        [TearDown]
        public void Stop()
        {
            this.driver.Quit();
            this.driver = null;
        }
    }
}
