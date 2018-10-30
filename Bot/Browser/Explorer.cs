using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using Bot.Instagram;

namespace Bot.Browser
{
    /// <summary>
    /// Base methods to call on WebDriver object.
    /// </summary>
    public abstract class Explorer
    {
        private Driver _browserDriver;
        protected ChromeDriver WebDriver;
        protected Config _config;

        public Explorer(Config config)
        {
            _browserDriver = Driver.GetInstance();

            _browserDriver.SetProxy(config.Proxy.IpAddress, config.Proxy.Port, config.Proxy.Username, config.Proxy.Password);
            _browserDriver.IsRandomMobileWindowSize = config.RandomWindowSize;
            _browserDriver.SetUserAgent(config.UserAgent);

        }

        public void Open()
        {
            _browserDriver.Open();
            WebDriver = _browserDriver.GetDriver();
        }

        public void Close()
        {
            _browserDriver.Close();
            WebDriver = null;
        }

        public void SetWindowSize(int width, int height)
        {
            WebDriver.Manage().Window.Size = new System.Drawing.Size(width, height);
        }

        public void GoBack()
        {
            WebDriver.Navigate().Back();
        }

        public void Wait(int seconds)
        {
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
        }

        public OpenQA.Selenium.Support.UI.WebDriverWait WaitForElement(int seconds)
        {
            TimeSpan span = TimeSpan.FromSeconds(seconds);
            OpenQA.Selenium.Support.UI.WebDriverWait wait = new OpenQA.Selenium.Support.UI.WebDriverWait(WebDriver, span);

            //IWebElement foundElement = wait.Until(web => WebDriver.FindElement(By.XPath(xPath)));
            //return foundElement;
            return wait;
        }

        public void SendKeys(string keys)
        {
            Actions actions = new Actions(WebDriver);
            actions.SendKeys(keys).Perform();
        }

        public void ExecuteJsClick(IWebElement element)
        {
            IJavaScriptExecutor ex = (IJavaScriptExecutor)WebDriver;
            ex.ExecuteScript("arguments[0].click();", element);
        }

        public void OpenPage(string url)
        {
            WebDriver.Url = url;
            WebDriver.Navigate();
            Wait(5);
        }

        public void OpenNewTabPage(string url)
        {
            WebDriver.ExecuteScript("window.open();");
            WebDriver.SwitchTo().Window(WebDriver.WindowHandles.Last());
            WebDriver.Navigate().GoToUrl(url);
            Wait(2);
        }
        
        /**
         * Should be homepage as opened first by default... 
         */
        public void GoBackToFirstPage()
        {
            WebDriver.SwitchTo().Window(WebDriver.WindowHandles.First());
            Wait(2);
        }

        public void CloseCurrentTab()
        {
            WebDriver.ExecuteScript("window.close();");
            WebDriver.SwitchTo().Window(WebDriver.WindowHandles.First());
        }

        public void Scroll(int position, int wait)
        {
            string command = "scroll(0," + position.ToString() + ");";
            WebDriver.ExecuteScript(command);
            Wait(wait);
        }

        public void ScrollToBottom()
        {
            WebDriver.ExecuteScript("scroll(0, document.body.scrollHeight)");
            Wait(10);
        }
    }
}
