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

    public class Cart : Application
    {
        public ReadOnlyCollection<IWebElement> checkoutList = Driver.FindElements(By.XPath(".//table[contains(@class,'dataTable')]//td[@class='item']"));

        public void DeleteAllProducts()
        {
            var checkoutListCount = checkoutList.Count;
            for (; checkoutListCount > 0; checkoutListCount--)
            {
                IWebElement removeButton = new WebDriverWait(Driver, TimeSpan.FromSeconds(20))
                    .Until<IWebElement>((d) => d.FindElement(By.XPath(".//button[@name='remove_cart_item']")));
                removeButton.Click();
                new WebDriverWait(Driver, TimeSpan.FromSeconds(10)).Until(
                    d => d.FindElements(By.XPath(".//table[contains(@class,'dataTable')]//td[@class='item']")).Count < checkoutListCount);
                Assert.IsTrue(
                    checkoutListCount - Driver.FindElements(By.XPath(".//table[contains(@class,'dataTable')]//td[@class='item']")).Count == 1);

            }
        }

        public bool isCartEmpty()
        {
            return Driver.FindElements(By.XPath(".//em[text()='There are no items in your cart.']")).Count > 0;
        }
    }
}
