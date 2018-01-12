using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumNew3
{
    using System.Collections.ObjectModel;

    using OpenQA.Selenium;

    public class MainPage : Application
    {
        public ReadOnlyCollection<IWebElement> ProductItems = Driver.FindElements(By.XPath(".//li[contains(@class,'product column')]//a[@class='link']//div[@class='name']"));
        public IWebElement cartCounter = Driver.FindElement(By.XPath(".//span[@class='quantity']"));
        public IWebElement gotoCartLink = Driver.FindElement(By.XPath(".//div[@id='cart-wrapper']//a[@class='link']"));

        public Product OpenProduct(string productName)
        {
            if (productName == "random")
            {
                var rand = new Random().Next(0, ProductItems.Count);
                ProductItems[rand].Click();
            }
            else
            {
                foreach (var product in ProductItems)
                {
                    if (product.Text == productName)
                    {
                        product.Click();
                        break;
                    }
                }
            }
            return new Product();
        }
        public string GetCartItemsCount()
        {
            return Driver.FindElement(By.XPath(".//span[@class='quantity']")).GetAttribute("innerText");
        }
        public Cart OpenCart()
        {
            gotoCartLink.Click();
            return new Cart();
        }
    }
}
