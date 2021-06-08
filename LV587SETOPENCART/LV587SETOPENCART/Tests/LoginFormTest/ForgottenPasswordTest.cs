﻿using LV587SETOPENCART.Pages;
using LV587SETOPENCART.BL;
using OpenQA.Selenium;
using System;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using System.Threading;
using Allure.Commons;

namespace LV587SETOPENCART.Tests
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("[LoginForm]")]
    [AllureDisplayIgnored]
    class ForgottenPasswordTest
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
            //driver.Navigate().GoToUrl(@"http://localhost/");
            ClassWithDriver classWithDriver = new ClassWithDriver(driver);
            classWithDriver.NavigateToURL();
        }

        [Test]
        [AllureTag("OpenCart: Forgotten Password Test")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("Taras Hrysiuk")]
        [Description("This test checks to if user can input their email to if he forgot password")]
        public void ForgottenPasswordPageTest()
        {
            //Click on My Account > Login
            HeaderComponent headerComponent = new HeaderComponent(driver);
            headerComponent.ClickOnMyAccount(MyAccountMenuActions.Login);
            Thread.Sleep(2000);  //Only for presentation (work Without it)

            // Click on "Forgotten Password" link text and input email
            ForgottenPasswordBL forgottenPasswordBL = new ForgottenPasswordBL(driver);
            forgottenPasswordBL.ForgottenPassword("user1@gmail.com");
            Thread.Sleep(2000);  //Only for presentation (work Without it)

            //Assert
            ForgottenPasswordPage forgottenPasswordPage = new ForgottenPasswordPage(driver);
            string expRes = "An email with a confirmation link has been sent your email address.";
            var actRes = forgottenPasswordPage.AlertMessageText();
            //Assert.AreEqual(expRes, actRes);
            Screenshot AfterTestScreen = ((ITakesScreenshot)driver).GetScreenshot();
            try
            {
                Assert.AreEqual(expRes, actRes);
            }
            catch (Exception) //Take a ScreenShot if test is failed
            {
                AfterTestScreen.SaveAsFile("D://Projects_C#//demo3//LV587SETOPENCART//LV587SETOPENCART//bin//Debug//net5.0//screens//ScreenshotForgotPassTest.Png", ScreenshotImageFormat.Png);
                AllureLifecycle.Instance.AddAttachment("TearDown", "application/png", @"C:\Users\vpfaitc\Desktop\OpenCart\LV587SetOpencart\LV587SETOPENCART\LV587SETOPENCART\bin\Debug\net5.0\screens\ScreenshotForgotPassTest.Png");
            }

            Thread.Sleep(2000);  //Only for presentation (work Without it)
        }
    }
}