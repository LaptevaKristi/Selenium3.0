using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SeleniumNew3
{
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
            this.driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
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


        [TearDown]
        public void Stop()
        {
            this.driver.Quit();
            this.driver = null;
        }
    }
}
