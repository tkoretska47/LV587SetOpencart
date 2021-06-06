﻿using LV587SETOPENCART.Pages;
using LV587SETOPENCART.BL;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace LV587SETOPENCART.Tests
{
    [TestFixture]
    class CartPageTest
    {
        IWebDriver driver;
        [OneTimeSetUp]
        public void BeforeAllMethods()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Window.Maximize();
        }

        [OneTimeTearDown]
        public void AfterAllMethods()
        {
            driver.Quit();
        }

        [SetUp]
        public void SetUp()
        {
            driver.Navigate().GoToUrl(@"http://localhost");
        }

        [Test]
        public void Test1()
        {
            //Arrange
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            HeaderComponent phones = new HeaderComponent(driver);
            phones.ChooseCategory(CategoryMenu.PhonesAndPDAs);
            PageWithProducts phoneUnit = new PageWithProducts(driver);
            phoneUnit.ClickCartButton();
            wait.Until(webDriver => webDriver.FindElement(phones.CartButtonLabel).Displayed);
            Thread.Sleep(2000);// for presentation (everything works without it)
            //Act
            string act = phones.CartButtonLabelText();
            string exp = "1 item(s) - $122.00";
            //Assert
            Assert.AreEqual(exp, act);

            //Arrange
            phones.ClickOnShoppingCartBlackButton();
            CartButtonComponent textInCart = new CartButtonComponent(driver);
            Thread.Sleep(2000);// for presentation (everything works without it)
            //Act
            string actu = textInCart.GetProductNameInCart();
            string expe = phoneUnit.GetFirstProductName();
            //Assert
            Assert.AreEqual(expe, actu);

            //Arrange
            textInCart.RemoveButtonInCart();
            //Act
            string act_res = "0 item(s) - $0.00";
            string exp_res = phones.CartButtonLabelText();
            //Assert
            Assert.AreEqual(exp_res, act_res);
        }
        [Test]
        public void Test2()
        {
            WebDriverWait waits = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //Arrange
            HeaderComponent phones = new HeaderComponent(driver);
            LoginBL logIn = new LoginBL(driver);
            PageWithProducts phoneUnit = new PageWithProducts(driver);
            CartPage input = new CartPage(driver);
            //Act
            phones.ClickOnMyAccount(MyAccountMenuActions.Login);
            logIn.Login("hdhgsdh1@gmail.com", "hdhgsdh");
            phones.ChooseCategory(CategoryMenu.PhonesAndPDAs);
            phoneUnit.ClickCartButton();
            phones.ClickOnShoppingCartLink();
            input.QuantityInput("55");
            input.RefreshButtonClick();
            string modifiedCart = input.GetRefreshMessageText();
            //Assert
            Assert.IsTrue(modifiedCart.Contains("You have modified your shopping cart"));
            
            //Arrange & Act
            string expected = "$5,500.00";
            string actual = input.GetTotalPrice(); ;
            //Assert
            Assert.AreEqual(expected, actual);
            //Arrange
            input.RemoveCircleInCartButton();
            waits.Until(webDriver => webDriver.FindElement(By.CssSelector(".pull-right > a[href*='home']")));
            //Act
            string actualRes = input.EmptyCartMessage();
            //Assert
            Assert.IsTrue(actualRes.Contains("Your shopping cart is empty!"));
            Thread.Sleep(2000);// for presentation (everything works without it)
        }
    }
}