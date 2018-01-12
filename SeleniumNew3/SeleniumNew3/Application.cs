using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumNew3
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support.UI;

    public class Application
    {
        public static IWebDriver Driver;
        public WebDriverWait Wait;

        public Application()
        {
            if(Driver != null) return;

            Driver = new ChromeDriver();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            this.Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
        }
        public MainPage OpenMainPage()
        {
            Driver.Navigate().GoToUrl("http://localhost:8084/litecart/en/");
            return new MainPage();
        }
    }
}
