using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace N11_Keytorc
{
    class N11_Keytorc
    {
        string Url = "http://www.n11.com/";
        IWebDriver driver;

        [SetUp]
        public void startBrowser()
        {
            driver = new ChromeDriver("C://Users//pc//Desktop");
            driver.Navigate().GoToUrl(Url);
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Login()
        {
            var UserName = "UserID";
            var Password = "Password";

            IWebElement Btn_Login = driver.FindElement(By.CssSelector(".btnSignIn"));
            Btn_Login.Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("email")).SendKeys(UserName);
            Thread.Sleep(1000);

            driver.FindElement(By.Id("password")).SendKeys(Password);
            Thread.Sleep(1000);

            IWebElement Btn_UserLogin = driver.FindElement(By.Id("loginButton"));
            Btn_UserLogin.Click();
            Thread.Sleep(2000);

            IWebElement Kontrol = driver.FindElement(By.XPath(".//*[@id='header']/div/div/div[2]/div[2]/div[2]/div[1]/a[1]"));
            Assert.IsTrue(Kontrol.Text.Equals("Hesabım"), "Kullanıcı girişi başarısız");
            Thread.Sleep(3000);
            Console.WriteLine("Kullanıcı girişi sağlandı");
        }

        [Test]
        public void SearchSamsung()
        {
            Login();

            string searchWord = "Samsung";

            driver.FindElement(By.Id("searchData")).SendKeys(searchWord);
            Thread.Sleep(1000);

            IWebElement Btn_Search = driver.FindElement(By.ClassName("searchBtn"));
            Btn_Search.Click();
            Thread.Sleep(2000);

            IWebElement Search_Result_Text = driver.FindElement(By.ClassName("resultText"));
            Assert.IsTrue(Search_Result_Text.Text.Contains(searchWord), "Arama sonucu başarısız");
            Thread.Sleep(3000);
            Console.WriteLine("Arama sonucu görüntülendi");
        }

        [Test]
        public void Paging()
        {
            SearchSamsung();

            IWebElement Paging_Page2 = driver.FindElement(By.XPath(".//*[@id='contentListing']/div/div/div[2]/div[3]/a[2]"));
            Paging_Page2.Click();
            Thread.Sleep(2000);

            String PageNumber = driver.FindElement(By.ClassName("currentPage")).GetAttribute("value").ToString();
            Assert.True(PageNumber.Equals("2"), "2. Sayfaya ulaşılamadı");
            Thread.Sleep(3000);
            Console.WriteLine("2. Sayfa görüntülendi");
        }

        [Test]
        public void AddFavourite()
        {
            Paging();

            IWebElement List_3rdItem = driver.FindElement(By.XPath(".//*[@id='view']/ul/li[3]/div/div"));
            IWebElement ClickItem = List_3rdItem.FindElement(By.ClassName("productName"));
            var fav_item = ClickItem.Text;
            ClickItem.Click();
            Thread.Sleep(2000);
            Console.WriteLine("Ürün seçildi");

            IWebElement fav_item_title = driver.FindElement(By.CssSelector(".proName"));
            Assert.IsTrue(fav_item_title.Text.Contains(fav_item), "Ürün detayı gösterilemedi");
            Thread.Sleep(2000);
            Console.WriteLine("Ürün detayı gösterildi");

            IWebElement SelectBox = driver.FindElement(By.XPath(".//*[@id='skuArea']/fieldset"));
            IWebElement SelectColorCombo = SelectBox.FindElement(By.TagName("select"));
            SelectElement selectColor = new SelectElement(SelectColorCombo);
            selectColor.SelectByIndex(1);
            Thread.Sleep(1000);
            Console.WriteLine("Ürün rengi seçildi");

            IWebElement favourite_add = driver.FindElement(By.CssSelector(".addWishListBtn"));
            favourite_add.Click();
            Thread.Sleep(1000);
            IWebElement favourite_added = driver.FindElement(By.CssSelector(".addToFavorites.item"));
            favourite_added.Click();
            Thread.Sleep(1000);
            Console.WriteLine("Ürün favorilere eklendi");

            IWebElement add_ok = driver.FindElement(By.CssSelector(".btn.btnBlack.confirm"));
            add_ok.Click();
            Thread.Sleep(1000);

            IWebElement account = driver.FindElement(By.XPath(".//*[@id='header']/div/div/div[2]/div[2]/div[2]/div/a[1]"));
            account.Click();
            Thread.Sleep(2000);
            IWebElement my_account = driver.FindElement(By.XPath(".//*[@id='myAccount']/div/div/div[2]/ul/li[6]/a[1]"));
            my_account.Click();
            Thread.Sleep(2000);
            IWebElement my_favourite = driver.FindElement(By.XPath(".//*[@id='myAccount']/div[3]/ul/li[1]/div/a[1]"));
            my_favourite.Click();
            Thread.Sleep(2000);
            Console.WriteLine("Favori ürünler görüntülendi");

            IWebElement fav_items = driver.FindElement(By.XPath(".//*[@id='view']/ul/li[1]/div/div/a[1]"));
            IWebElement productDetail = fav_items.FindElement(By.ClassName("productName"));
            var title_new = productDetail.Text;
            Assert.That(fav_item, Is.EqualTo(title_new));
            Console.WriteLine("Sepetteki ürün ile seçilen ürünün aynı ürün olduğu doğrulandı");

            IWebElement Btn_Sil = driver.FindElement(By.ClassName("deleteProFromFavorites"));
            Btn_Sil.Click();
            Thread.Sleep(2000);
            Console.WriteLine("Ürün silme işlemi gerçekleştirildi");

            IWebElement add_okey = driver.FindElement(By.CssSelector(".btn.btnBlack.confirm"));
            add_okey.Click();
            Thread.Sleep(1000);

            IWebElement delete_ok = driver.FindElement(By.XPath(".//*[@id='myAccount']/div[3]/div/div"));
            IWebElement delete_okey = delete_ok.FindElement(By.ClassName("emptyWatchList"));
            Assert.True(delete_okey.Text.Contains("İzlediğiniz bir ürün bulunmamaktadır."));
            Thread.Sleep(2000);
            Console.WriteLine("Ürün silinme onayı alındı");
        }

        [TearDown]
        public void closeBrowser()
        {
            driver.Close();
            driver.Quit();
        }
    }
}