using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SeleniumNew3
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using NUnit.Framework;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Remote;
    using OpenQA.Selenium.Support.UI;

    using SeleniumNew3.Objects;

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
        
        [Test]
        public void Zad9_ValidateSortCountryAndGeoZoneInAdmin()
        {

            //Zadanie 9_1
            this.LoginInAdminPanel();

            this.driver.Navigate().GoToUrl($"{AdminLiteCartUrl}/?app=countries&doc=countries");

            var countryList = this.driver.FindElements(By.XPath(".//table[@class='dataTable']//a[not(i)]"));
            var countryListName = countryList.Select(c => c.Text).ToList();

            var sortedCountryListName = countryListName.ToList();
            sortedCountryListName.Sort();
            Assert.AreEqual(sortedCountryListName, countryListName);
            
            var zones = this.driver.FindElements(By.XPath(".//table[@class='dataTable']//tr[@class='row']/td[last()-1]"));

            var dictionaryCountry = new Dictionary<string, string>();
            for (var i = 0; i < countryListName.Count; i++)
            {
                if (Convert.ToInt32(zones[i].Text) != 0)
                {
                    dictionaryCountry.Add(countryListName[i], zones[i].Text);
                }
            }

            foreach (var country in dictionaryCountry)
            {
                this.driver.FindElement(By.XPath($".//table[@class='dataTable']//a[text()='{country.Key}']")).Click();
                var zonesCountry = this.driver.FindElements(By.XPath(".//table[@id='table-zones']//input[contains(@name, 'name')]/.."));
                var zonesCountrywithoutEmpty = zonesCountry.Where(z => !string.IsNullOrEmpty(z.Text)).ToList();
                var zonesCountryText = zonesCountrywithoutEmpty.Select(z => z.Text).ToList();
                var sortedZonesListName = zonesCountryText.ToList();
                sortedZonesListName.Sort();
                Assert.AreEqual(sortedZonesListName, zonesCountryText);
                
                this.driver.FindElement(By.XPath(".//button[@name='cancel']")).Click();
                this.wait.Until(ExpectedConditions.ElementExists(By.XPath(".//h1[contains(text(), 'Countries')]")));
            }

            //Zadanie 9_2
            this.driver.Navigate().GoToUrl($"{AdminLiteCartUrl}/?app=geo_zones&doc=geo_zones");

            var geoZoneList =
                this.driver.FindElements(By.XPath(".//table[@class='dataTable']//tr[@class='row']/td[last()-2]"));
            var geoZoneNames = geoZoneList.Select(g => g.Text).ToList();

            foreach (var name in geoZoneNames)
            {
               this.driver.FindElement(By.XPath($".//a[text()='{name}']")).Click();
                var zonesList = driver.FindElements(
                    By.XPath($".//select[contains(@name, 'zone_code')]/option[@selected='selected']"));

                var zonesName = zonesList.Select(z => z.Text).ToList();
                var sortedZonesName = zonesName.ToList();
                sortedZonesName.Sort();
                    Assert.AreEqual(sortedZonesName, zonesName);

                this.driver.FindElement(By.XPath(".//button[@name='cancel']")).Click();
                this.wait.Until(ExpectedConditions.ElementExists(By.XPath(".//h1[contains(text(), 'Geo Zones')]")));
            }
        }

        [Test]
        public void Zad10_ValidateOpenPage()
        {
            this.driver.Navigate().GoToUrl(LiteCartUrl);
            
            var listOfProducts = new List<Duck>();
            var products = this.driver.FindElements(By.XPath(".//li[contains(@class, 'product column')]"));

            foreach (var product in products)
            {
                var url = product.FindElement(By.XPath(".//a"));
                var name = product.FindElement(By.XPath(".//div[@class='name']"));
                var manufacturer = product.FindElement(By.XPath(".//div[@class='manufacturer']"));
                var price = product.FindElements(By.XPath(".//span[contains(@class,'price')]")).Count == 0
                                ? product.FindElement(By.XPath(".//s[contains(@class,'price')]"))
                                : product.FindElement(By.XPath(".//span[contains(@class,'price')]"));
                var saleprice = product.FindElements(By.XPath(".//strong[contains(@class,'price')]")).Count == 0
                                    ? null
                                    : product.FindElement(By.XPath(".//strong[contains(@class,'price')]"));
                listOfProducts.Add(
                    new Duck()
                        {
                            URL = url.GetAttribute("href"),
                            Name = name.Text,
                            Manufacturer = manufacturer.Text,
                            Price =
                                new Price()
                                    {
                                        Amount = Int64.Parse(price.Text.Replace("$", "")),
                                        Color = price.GetCssValue("color").Substring(5).Replace(")", "")
                                            .Split(new char[] { ',' }).ToArray()
                                            .Select(x => Int32.Parse(x)).ToList(),
                                        Size = Int32.Parse(price.GetCssValue("font-size").Remove(2)),
                                        Style = price.GetCssValue("text-decoration-line")
                                    },
                            SalePrice = saleprice == null
                                            ? null
                                            : new Price()
                                                  {
                                                      Amount =
                                                          Int64.Parse(saleprice.Text.Replace("$", "")),
                                                      Color = saleprice.GetCssValue("color").Substring(5)
                                                          .Replace(")", "").Split(new char[] { ',' })
                                                          .ToArray().Select(x => Int32.Parse(x))
                                                          .ToList(),
                                                      Size = Int32.Parse(
                                                          saleprice.GetCssValue("font-size").Remove(2)),
                                                      Style = saleprice.GetCssValue("font-weight")
                                                  }
                        });
            }

            foreach (var productOnMain in listOfProducts)
                {
                    driver.Navigate().GoToUrl(productOnMain.URL);
                    var nameDuck = driver.FindElement(By.XPath(".//h1"));
                    var manufacturerDuck = driver.FindElement(By.XPath(".//div[@class='manufacturer']//img"));
                    var priceCount = driver.FindElements(
                        By.XPath(".//div[@class='information']//span[contains(@class,'price')]")).Count;
                    var priceDuck =  priceCount== 0 ?
                                     driver.FindElement(By.XPath(".//div[@class='information']//s[contains(@class,'price')]")) :
                                     driver.FindElement(By.XPath(".//div[@class='information']//span[contains(@class,'price')]"));
                    var salepriceDuck = driver.FindElements(By.XPath(".//div[@class='information']//strong[contains(@class,'price')]")).Count == 0 ?
                                         null :
                                         driver.FindElement(By.XPath(".//div[@class='information']//strong[contains(@class,'price')]"));
                    
                    Assert.IsTrue(nameDuck.Text == productOnMain.Name, "Wrong  name!");
                    Assert.IsTrue(manufacturerDuck.GetAttribute("Title") == productOnMain.Manufacturer, "Wrong manufacturer");
                    
                    Assert.IsTrue(Int64.Parse(priceDuck.Text.Replace("$", "")) == productOnMain.Price.Amount, "Wrong price");
                    
                    var color = priceDuck.GetCssValue("color").Substring(5).Replace(")", "").Split(new char[] { ',' }).ToList()
                                                        .Select(x => Int32.Parse(x)).ToList();
                    Assert.IsTrue(color[0] == color[1] && color[1] == color[2], "Wrong price color");
                    
                    if (salepriceDuck != null && productOnMain.SalePrice != null)
                     {
                         Assert.IsTrue(priceDuck.GetCssValue("text-decoration-line") == productOnMain.Price.Style, "Wrong price text style");
                        Assert.IsTrue(Int64.Parse(salepriceDuck.Text.Replace("$", "")) == productOnMain.SalePrice.Amount);
                        Assert.IsTrue(salepriceDuck.GetCssValue("font-weight") == productOnMain.SalePrice.Style, "Wrong price text style");
                        var salecolor = salepriceDuck.GetCssValue("color").Substring(5).Replace(")", "").Split(new char[] { ',' }).ToList()
                                                                    .Select(x => Int32.Parse(x)).ToList();
                        Assert.IsTrue(salecolor[1] == salecolor[2] && salecolor[1] == 0, "Wrong sale price color");
                        Assert.IsTrue(Int32.Parse(salepriceDuck.GetCssValue("font-size").Remove(2)) > Int32.Parse(priceDuck.GetCssValue("font-size").Remove(2)), "Wrong sale price size");
                        Assert.IsTrue(Int64.Parse(priceDuck.Text.Replace("$", "")) > Int64.Parse(salepriceDuck.Text.Replace("$", "")));
                     }
                 }
        }

        [Test]
        public void Zad11_RegisterNewUser()
        {
            this.driver.Url = LiteCartUrl;
            this.driver.FindElement(By.XPath(".//a[text()='New customers click here']")).Click();

            var FirstName = driver.FindElement(By.XPath(".//input[@name='firstname']"));
            var LastName = driver.FindElement(By.XPath(".//input[@name='lastname']"));
            var Address1 = driver.FindElement(By.XPath(".//input[@name='address1']"));
            var Postcode = driver.FindElement(By.XPath(".//input[@name='postcode']"));
            var City = driver.FindElement(By.XPath(".//input[@name='city']"));
            SelectElement Country =
                new SelectElement(driver.FindElement(By.XPath(".//select[@name='country_code']")));
            SelectElement ZoneCode =
                new SelectElement(driver.FindElement(By.XPath(".//select[@name='zone_code']")));
            var Email = driver.FindElement(By.XPath(".//input[@name='email']"));
            var Phone = driver.FindElement(By.XPath(".//input[@name='phone']"));
            var Password = driver.FindElement(By.XPath(".//input[@name='password']"));
            var PasswordConfirm = driver.FindElement(By.XPath(".//input[@name='confirmed_password']"));
            var CreateButton = driver.FindElement(By.XPath(".//button[@name='create_account']"));

            var UniqEmail = GiveUniqEmail();

            FirstName.SendKeys("Name");
            LastName.SendKeys("Last");
            Address1.SendKeys("Address");
            Postcode.SendKeys("12345");
            City.SendKeys("City");
            Country.SelectByText("United States");
            new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                ExpectedConditions.ElementExists(By.XPath(".//select[@name='zone_code' and not(@disabled)]")));
            ZoneCode.SelectByText("Florida");
            Email.SendKeys(UniqEmail);
            Phone.SendKeys("+10000000000");
            Password.SendKeys("password");
            PasswordConfirm.SendKeys("password");
            CreateButton.Click();
            Assert.IsTrue(driver.FindElements(By.XPath(".//div[@class='notice success']")).Count > 0);

            new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                ExpectedConditions.ElementExists(By.XPath(".//div[@id='box-account']//a[text()='Logout']")));
            Logout(driver);
            Login(driver, UniqEmail, "password");
            new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                ExpectedConditions.ElementExists(By.XPath(".//div[@id='box-account']//a[text()='Logout']")));
            Logout(driver);
        }

        [Test]
        public void Zad12_AddNewProductInAdmin()
        {
            this.LoginInAdminPanel();
            driver.Navigate().GoToUrl($"{AdminLiteCartUrl}/?app=catalog&doc=catalog");
            var UniqNumber = GiveUniqNum();
            var UniqName = "Another Duck №" + UniqNumber;
            var AddProduct = driver.FindElement(By.XPath(".//a[@class='button' and contains(text(),'Add New Product')]"));
            AddProduct.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                ExpectedConditions.ElementExists(By.XPath(".//div[@class='tabs']")));

            //general
            var status = driver.FindElement(By.XPath(".//input[@name='status' and @value='1']"));
            var name = driver.FindElement(By.XPath(".//strong[text()='Name']/..//input"));
            var Code = driver.FindElement(By.XPath(".//strong[text()='Code']/..//input"));
            var Category = driver.FindElement(By.XPath(".//strong[text()='Categories']/..//input[@data-name='Root']"));
            var ProductGroupUnisex = driver.FindElement(By.XPath(".//strong[text()='Product Groups']/..//td[text()='Unisex']/..//input"));
            var Quantity = driver.FindElement(By.XPath(".//input[@name='quantity']"));
            var UploadImage = driver.FindElement(By.XPath(".//strong[text()='Upload Images']/..//input"));
            var DateValidFrom = driver.FindElement(By.XPath(".//strong[text()='Date Valid From']/..//input"));
            var DateValidTo = driver.FindElement(By.XPath(".//strong[text()='Date Valid To']/..//input"));

            if (!status.Selected) { status.Click(); }
            name.SendKeys(UniqName);
            Code.SendKeys(UniqNumber);
            if (!Category.Selected) { Category.Click(); }
            if (!ProductGroupUnisex.Selected) { ProductGroupUnisex.Click(); }
            Quantity.Clear();
            Quantity.SendKeys("10");
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Penguins.jpg"); 
            UploadImage.SendKeys(path);
            DateValidFrom.SendKeys("01.12.2017");
            DateValidTo.SendKeys("01.02.2018");

            //information
            var InfoTab = driver.FindElement(By.XPath(".//div[@class='tabs']//li//a[text()='Information']"));
            InfoTab.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                ExpectedConditions.ElementExists(By.XPath(".//div[@id='tab-information' and @style='display: block;']")));
            var Manufacturer = driver.FindElement(By.XPath(".//select[@name='manufacturer_id']"));
            var Keywords = driver.FindElement(By.XPath(".//input[@name='keywords']"));
            var ShortDescription = driver.FindElement(By.XPath(".//input[@name='short_description[en]']"));
            var Description = driver.FindElement(By.XPath(".//textarea[@name='description[en]']"));
            var HeadTitle = driver.FindElement(By.XPath(".//input[@name='head_title[en]']"));

            var selectManufacturer = new SelectElement(Manufacturer);
            selectManufacturer.SelectByValue("1");
            Keywords.SendKeys("Red;Duck");
            ShortDescription.SendKeys("Another Red Duck");
            Description.SendKeys("Another Red Duck with 10$ price! BUY NOW!");
            HeadTitle.SendKeys("Red Duck");

            //price
            var PriceTab = driver.FindElement(By.XPath(".//div[@class='tabs']//li//a[text()='Prices']"));
            PriceTab.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                ExpectedConditions.ElementExists(By.XPath(".//div[@id='tab-prices' and @style='display: block;']")));
            var Price = driver.FindElement(By.XPath(".//input[@name='purchase_price']"));
            var Money = driver.FindElement(By.XPath(".//select[@name='purchase_price_currency_code']"));
            var SaveButton = driver.FindElement(By.XPath(".//button[@name='save']"));

            Price.Clear();
            Price.SendKeys("10");
            var SelectMoney = new SelectElement(Money);
            SelectMoney.SelectByValue("USD");

            SaveButton.Click();

            new WebDriverWait(driver, TimeSpan.FromSeconds(15)).Until(
                ExpectedConditions.ElementExists(By.XPath(".//table[@class='dataTable']")));
            var H1elemennt = driver.FindElement(By.XPath(".//h1"));
            Assert.IsTrue(H1elemennt.Text == "Catalog");
            var CreatedProduct = driver.FindElements(By.XPath(".//table[@class='dataTable']//tr[@class='row']//a[text()='" + UniqName + "']"));
            Assert.IsTrue(CreatedProduct.Count > 0);
        }

        [Test]
        public void Zad13_WorkWithCart()
        {
            this.driver.Url = LiteCartUrl;
            var cartCounter = Int32.Parse(driver.FindElement(By.XPath(".//span[@class='quantity']")).GetAttribute("innerText"));

            while (cartCounter < 3)
            {
                driver.Navigate().GoToUrl(LiteCartUrl);
                var ProductItem =
                    driver.FindElement(By.XPath(".//li[contains(@class,'product column')]//a[@class='link']"));
                ProductItem.Click();
                if (driver.FindElements(By.XPath(".//select[@name='options[Size]']")).Count > 0)//если есть поле "размер" которое обязательно для заполнения
                {
                    new SelectElement(driver.FindElement(By.XPath(".//select[@name='options[Size]']"))).SelectByValue("Small");
                }
                var add2CartButton = driver.FindElement(By.XPath(".//button[@name='add_cart_product']"));
                var quantityBefore = driver.FindElement(By.XPath(".//span[@class='quantity']"))
                    .GetAttribute("innerText");
                add2CartButton.Click();
                new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until((IWebDriver d) => d.FindElement(By.XPath(".//span[@class='quantity']")).GetAttribute("innerText") != quantityBefore);
                Assert.IsTrue(Int32.Parse(driver.FindElement(By.XPath(".//span[@class='quantity']")).GetAttribute("innerText")) -
                              cartCounter == 1);
                cartCounter++;
            }

            var checkoutLink = driver.FindElement(By.XPath(".//div[@id='cart-wrapper']//a[@class='link']"));
            checkoutLink.Click();

            var checkoutListCount = driver.FindElements(By.XPath(".//table[contains(@class,'dataTable')]//td[@class='item']")).Count;
            for (; checkoutListCount > 0; checkoutListCount--)
            {
                var removeButton = driver.FindElement(By.XPath(".//button[@name='remove_cart_item']"));
                removeButton.Click();
                new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(
                    d => d.FindElements(By.XPath(".//table[contains(@class,'dataTable')]//td[@class='item']")).Count < checkoutListCount);
                Assert.IsTrue(
                    checkoutListCount - driver.FindElements(By.XPath(".//table[contains(@class,'dataTable')]//td[@class='item']")).Count == 1);

            }
            Assert.IsTrue(
                driver.FindElements(By.XPath(".//em[text()='There are no items in your cart.']")).Count > 0);
        }

        private string GiveUniqEmail()
        {
            return DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + "@gmail.com";
        }

        public string GiveUniqNum()
        {
            return (DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond).ToString();
        }

        private void Login(IWebDriver driver, string Login, string Password)
        {
            try
            {
                var Email = driver.FindElement(By.XPath(".//input[@name='email']"));
                var Pass = driver.FindElement(By.XPath(".//input[@name='password']"));
                var LoginBtn = driver.FindElement(By.XPath(".//button[@name='login']"));

                Email.SendKeys(Login);
                Pass.SendKeys(Password);
                LoginBtn.Click();
                Assert.IsTrue(driver.FindElements(By.XPath(".//div[@class='notice success'and contains(text(),' You are now logged in as')]")).Count > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.InnerException.Message);
                throw;
            }
        }

        private void Logout(IWebDriver driver)
        {
            try
            {
                var Logout = driver.FindElement(By.XPath(".//div[@id='box-account']//a[text()='Logout']"));
                Logout.Click();
                Assert.IsTrue(driver.FindElements(By.XPath(".//div[@class='notice success'and text()=' You are now logged out.']")).Count > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message + "\n" + e.InnerException.Message);
                throw;
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
