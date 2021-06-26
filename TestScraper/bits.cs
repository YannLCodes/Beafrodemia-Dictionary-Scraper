using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;

namespace TestScraper
{
    [TestClass]
    public class bits
    {
        FirefoxDriver _localDriver;
        RemoteWebDriver _remoteDriver;
        public bits()
        {
            //_localDriver = new FirefoxDriver();
            //var remoteHub = "http://localhost:4444/wd/hub"; //<==default selenium hub location
            //FirefoxOptions firefoxOptions = new FirefoxOptions();
            //IWebDriver _remoteDriver = new RemoteWebDriver(firefoxOptions);

        }

        [TestMethod]
        public void LocalTest()
        {
            _localDriver.Manage().Window.Maximize();
            _localDriver.Navigate().GoToUrl("https://www.youtube.com/channel/UCCYR9GpcE3skVnyMU8Wx1kQ");
            _localDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            Assert.IsTrue(_localDriver.PageSource.Contains(value: "Houssem"));
        }

        [TestMethod]
        public void RemoteTest()
        {
            var remoteHub = "http://localhost:4444/wd/hub"; //<==default selenium hub location
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            IWebDriver _remoteDriver = new RemoteWebDriver(firefoxOptions);

            _remoteDriver.Manage().Window.Maximize();
            //_remoteDriver.Navigate().GoToUrl("https://www.youtube.com/channel/UCCYR9GpcE3skVnyMU8Wx1kQ");
            _remoteDriver.Navigate().GoToUrl("https://www.ecosia.com/");
            _remoteDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            _remoteDriver.Quit();
            //Assert.IsTrue(_remoteDriver.PageSource.Contains(value: "Houssem"));
        }
    }
}
