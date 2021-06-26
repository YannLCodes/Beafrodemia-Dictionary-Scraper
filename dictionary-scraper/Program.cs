using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dictionary_scraper
{
    class Program
    {
        static void Main(string[] args)
        {

            IWebDriver driver = new FirefoxDriver();

            driver.Url = "http://joelandjes.com/sangolex/lexicon/main.htm";

            driver.Navigate().GoToUrl("http://joelandjes.com/sangolex/index-english/01.htm");


            //EN dictionary --Dashed definitions are multiple translations for a single word
            http://joelandjes.com/sangolex/index-english/01.htm

            /*FR dictionary -- Dashed definitions are multiple translations for a single word
            http://joelandjes.com/sangolex/index-french/01.htm*/

            //Sango dictionary
            //http://joelandjes.com/sangolex/lexicon/01.htm

            IWebElement bodyFrame = driver.FindElement(By.TagName("body"));

            IList<IWebElement> elements = bodyFrame.FindElements(By.TagName("tr"));//.Where(i => i.GetCssValue);
            foreach (IWebElement e in elements)
            {
                var BaseTermEnglish = e.FindElement(By.ClassName("lpIndexEnglish")).Size.IsEmpty ? "-" : e.FindElement(By.ClassName("lpIndexEnglish")).Text;
                var foo = new Dictionary<string, string>()
                {
                    { "BaseTermEnglish", BaseTermEnglish },
                    { "TermSango", e.FindElement(By.ClassName("lpLexEntryName")).Text },
                };

                Console.WriteLine(
                    $"BaseTermEnglish : {e.FindElement(By.ClassName("lpIndexEnglish")).Text }, TermSango : {e.FindElement(By.ClassName("lpLexEntryName")).Text }"
                    );

            }

            /* Dump into excel file, col for name   
            Sango || type || EN || FR */



            driver.Quit();
            //RunDockerisedScraper();

        }

        public static void RunDockerisedScraper()
        {
            var remoteHub = "http://localhost:4444/wd/hub"; //<==default selenium hub location
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            IWebDriver _remoteDriver = new RemoteWebDriver(firefoxOptions);

            _remoteDriver.Manage().Window.Maximize();
            _remoteDriver.Navigate().GoToUrl("http://joelandjes.com/sangolex/index-english/01.htm");
            _remoteDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            IWebElement bodyFrame = _remoteDriver.FindElement(By.TagName("body"));

           /* IList<IWebElement> elements = bodyFrame.FindElements(By.TagName("p")).Where(i => i.GetCssValue("yuh"));
            foreach (IWebElement e in elements)
            {
                System.Console.WriteLine(e.Text);

            }*/

            _remoteDriver.Quit();

        }
    }
}
