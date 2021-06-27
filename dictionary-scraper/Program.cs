using CsvHelper;
using dictionary_scraper.Models;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace dictionary_scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            //EN dictionary --Dashed definitions are multiple translations for a single word
            http://joelandjes.com/sangolex/index-english/01.htm

            /*FR dictionary -- Dashed definitions are multiple translations for a single word
            http://joelandjes.com/sangolex/index-french/01.htm*/

            //Sango dictionary
            //http://joelandjes.com/sangolex/lexicon/01.htm

            RunLocalScraperAsync();
           
            //RunDockerisedScraper();

        }

        public static void RunLocalScraperAsync()
        {
            IWebDriver driver = new FirefoxDriver();

            ScrapeDictionaryEntries(driver);

            driver.Quit();
        }

        public static void RunDockerisedScraper()
        {
            var remoteHub = "http://localhost:4444/wd/hub"; //<==default selenium hub location
            FirefoxOptions firefoxOptions = new FirefoxOptions();
            IWebDriver _remoteDriver = new RemoteWebDriver(firefoxOptions);
            ScrapeDictionaryEntries(_remoteDriver);
            _remoteDriver.Quit();

        }

        public static void ScrapeDictionaryEntries(IWebDriver driver)
        {
            driver.Url = "http://joelandjes.com/sangolex/lexicon/main.htm";
            int PageNumber = 1;
            var DictionaryTerms = new List<DictionaryTerm>();

            
            while(PageNumber < 27)
            {
                driver.Navigate().GoToUrl($"http://joelandjes.com/sangolex/index-english/{PageNumber.ToString("00")}.htm");
                IWebElement bodyFrame = driver.FindElement(By.TagName("body"));
                IList<IWebElement> elements = bodyFrame.FindElements(By.TagName("tr"));
                char LetterIndex = bodyFrame.FindElement(By.ClassName("lpTitlePara")).Text.First();
                string LastKnownBaseTerm = String.Empty;
                
                for (var i = 0; i < elements.Count(); i++)
                {
                    Console.WriteLine($"Dictionary Index Letter {LetterIndex}, processing entry [{i}/{elements.Count()}]");

                    string BaseTermEnglish;
                    string BaseTermSango = elements[i].FindElement(By.ClassName("lpLexEntryName")).Text;

                    try
                    {
                        BaseTermEnglish = elements[i].FindElement(By.ClassName("lpIndexEnglish")).Text;
                        LastKnownBaseTerm = BaseTermEnglish;

                        DictionaryTerms.Add(new DictionaryTerm()
                        {
                            BaseTerm = BaseTermEnglish,
                            Translation = new List<string>() { BaseTermSango }
                        });
                    }
                    catch (OpenQA.Selenium.NoSuchElementException)
                    {
                        BaseTermEnglish = LastKnownBaseTerm;
                        DictionaryTerms.Where(x => x.BaseTerm.ToLower() == LastKnownBaseTerm).First().Translation.Add(BaseTermSango);
                    }
                }
                SaveTranslations(DictionaryTerms);
                PageNumber++;
            }

            /* Dump into excel file, col for name   
            Sango || type || EN || FR */
        }

        public static void SaveTranslations(List<DictionaryTerm> DictionaryTerms)
        {
            var Dir = Directory.CreateDirectory("ScraperOutput");
            var DictionaryTermsJSON = JArray.FromObject(DictionaryTerms);
            var DictionaryTermsJSONDynamic = DictionaryTermsJSON.ToObject<ExpandoObject[]>();

            using (StreamWriter sw = new StreamWriter(Path.Combine(Dir.ToString(), $"Translations-{DateTime.Now.ToString("MM-dd-yyyy")}.json")))
            {
                sw.Write(DictionaryTermsJSON);
            }

            using (StreamWriter sw = new StreamWriter(Path.Combine(Dir.ToString(), $"Translations-{DateTime.Now.ToString("MM-dd-yyyy")}.csv")))
            using (var csv = new CsvWriter(sw, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(DictionaryTermsJSONDynamic as IEnumerable<dynamic>);
            }
        }
    }
}
