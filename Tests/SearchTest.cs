using System;
using System.Threading.Tasks;
using AllClassifieds.ScreenshotController;
using AllClassifieds.TestDataControllers;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace AllClassifieds
{
    [SetUpFixture]
    public abstract class Base
    {
        protected ExtentReports _extent;
        protected ExtentTest _test;
        private IWebDriver driver2;
        private IWebDriver driver;
        private TestDataController tdc = new TestDataController();
       

        [OneTimeSetUp]
        protected void Setup()
        {
            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;
            var htmlReporter = new ExtentHtmlReporter(projectPath + "\\Reports\\Report.html");

            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);

            htmlReporter.LoadConfig(projectPath + "\\extent-config.xml");
        }

        [OneTimeTearDown]
        protected void TearDown()
        {
            _extent.Flush();
            driver.Close();
            driver2.Close();
            driver.Quit();
            driver2.Quit();
        }


        [SetUp]
        public void Init()
        {
            driver = new ChromeDriver();
            driver2 = new ChromeDriver();
            
            _test = _extent.CreateTest(NUnit.Framework.TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void AfterTest()
        {
            var status = NUnit.Framework.TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(NUnit.Framework.TestContext.CurrentContext.Result.StackTrace)
                    ? ""
                    : string.Format("{0}", NUnit.Framework.TestContext.CurrentContext.Result.StackTrace);
            Status logstatus;
            var errorMessage = NUnit.Framework.TestContext.CurrentContext.Result.Message; 

            switch (status)
            {
                case TestStatus.Failed:
                    {
                        logstatus = Status.Fail;
                        string screenshotPath = GetScreenshot.Capture(driver, "Fail");                        
                        _test.Log(Status.Fail, "Snapshot below: ", MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());   
                        break;
                    }
                    
                case TestStatus.Inconclusive:
                    logstatus = Status.Warning;
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    break;
                default:
                    logstatus = Status.Pass;
                    break;
            }
            _test.Log(logstatus, "Test ended with " + logstatus + stacktrace);
            _extent.Flush();
        }

        [TestFixture]
        public class SearchTest : Base
        {           
            public void searchTest(IWebDriver driver, string url, string searchData, string country)
            {                
                //_test.
                driver.Navigate().GoToUrl(url);
                _test.Log(Status.Pass, "Navigated succesfully to the URL");

                IWebElement serachInput = driver.FindElement(By.Id("txtSearch"));
                serachInput.SendKeys(searchData);
                _test.Log(Status.Pass, "Passed search input successfully");

                IWebElement countryDropdown = driver.FindElement(By.XPath("//*[@id=\"ddlRegions\"]"));
                countryDropdown.SendKeys(country);
                _test.Log(Status.Pass, "Selected the country successfully");

                IWebElement searchButton = driver.FindElement(By.XPath("//*[@id=\"content\"]/div/form/button"));
                searchButton.Click();
                _test.Log(Status.Pass, "Clicked the search button");

                IWebElement resultList = driver.FindElement(By.Id("ResultsPageRight"));

                NUnit.Framework.Assert.IsTrue(resultList.Displayed,"Result page is displayed" );
                _test.Log(Status.Pass, "Result page is displayed");
            }

            [Test]
            public void searchTestSuite()
            {
                int n = tdc.GetLastTestIndex();

                var restul = Task.Run(() => 
                {
                    for (int i = 2; i <= n; i += 2)
                    {
                        string url = tdc.GetBaseURL(i);
                        string stringdata = tdc.GetSearchInput(i);
                        string country = tdc.GetCountry(i);
                        searchTest(driver2, url, stringdata, country);
                    }

                });
                var result2 = Task.Run(() => 
                {
                    for (int i = 3; i <= n; i += 2)
                    {
                        string url = tdc.GetBaseURL(i);
                        string stringdata = tdc.GetSearchInput(i);
                        string country = tdc.GetCountry(i);
                        searchTest(driver, url, stringdata, country);
                    }

                });
                result2.Wait();
                restul.Wait();
            }
        }
    }
}
