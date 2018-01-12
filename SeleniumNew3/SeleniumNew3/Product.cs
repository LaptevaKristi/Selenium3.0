using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumNew3
{
    using System.Collections.ObjectModel;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class Product : Application
    {
        public ReadOnlyCollection<IWebElement> SelectSize = Driver.FindElements(By.XPath(".//select[@name='options[Size]']"));
        public IWebElement add2CartButton = Driver.FindElement(By.XPath(".//button[@name='add_cart_product']"));

        public void Add2Cart()
        {
            if (SelectSize.Count > 0)//если есть поле "размер" которое обязательно для заполнения
            {
                new SelectElement(SelectSize[0]).SelectByValue("Small");
            }

            var quantityBefore = GetCartItemsCount();
            add2CartButton.Click();
            new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until((IWebDriver d) => d.FindElement(By.XPath(".//span[@class='quantity']")).GetAttribute("innerText") != quantityBefore);
            VerifyCartCountChanged(quantityBefore, GetCartItemsCount());
        }

        public string GetCartItemsCount()
        {
            return Driver.FindElement(By.XPath(".//span[@class='quantity']")).GetAttribute("innerText");
        }

        public void VerifyCartCountChanged(string before, string after)
        {
            Assert.IsTrue(Int32.Parse(after) - Int32.Parse(before) == 1);
        }
    }
}
