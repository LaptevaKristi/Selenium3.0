﻿using System;
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

        [SetUp]
        public void Start()
        {
            this.driver = new ChromeDriver();
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

        [TearDown]
        public void Stop()
        {
            this.driver.Quit();
            this.driver = null;
        }
    }
}