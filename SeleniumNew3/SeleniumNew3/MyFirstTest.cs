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

    [TestClass]
    public class MyFirstTest
    {
        private IWebDriver driver;

        private WebDriverWait wait;

        #region ConstVariables

        private const string AdminLiteCartUrl = "http://localhost:8084/litecart/admin";
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

        [TearDown]
        public void Stop()
        {
            this.driver.Quit();
            this.driver = null;
        }
    }
}
