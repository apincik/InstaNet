using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Bot.Browser.Extensions;

namespace Bot.Browser
{
    /// <summary>
    /// Browser driver definition, using ChromeDriver.
    /// </summary>
    public class Driver
    {
        private static readonly Driver _instance = new Driver();

        private ChromeDriver WebDriver;

        private string _userAgent = "";

        private int _windowWidth = 360;

        private int _windowHeight = 640;

        private ChromeOptions _options;

        public bool IsRandomMobileWindowSize = true;

        private bool _isOpened = false;

        //When false, start maximized or default.
        //@deprecated Value true is required.
        public bool IsMobileSize = true;

        private Driver()
        {
            _options = new ChromeOptions();
            _options.AddArgument("profile-directory=Default");
            _options.PageLoadStrategy = PageLoadStrategy.Normal;
        }
        
        public static Driver GetInstance()
        {
            return _instance;
        }

        public ChromeDriver GetDriver()
        {
            return WebDriver;
        }

        private void AddExtension(Extensions.Extensions extension)
        {
            _options.AddExtension(extension.Create());
        }

        public void Open()
        {
            if(_isOpened)
            {
                return;
            }

            _options.AddArgument(_userAgent);
            WebDriver = new ChromeDriver(_options);
            _isOpened = true;

            //Set window size as mobile window. Mobile user-agent is preffered.
            if (IsMobileSize)
            {
                if (IsRandomMobileWindowSize)
                {
                    Random randNum = new Random();
                    _windowWidth = randNum.Next(360, 414);
                    _windowHeight = randNum.Next(640, 736);
                }

                WebDriver.Manage().Window.Size = new System.Drawing.Size(_windowWidth, _windowHeight);
            }
        }

        public void Close()
        {
            WebDriver.Close();
            _isOpened = false;
        }

        public void SetProxy(string ipAddress, string port, string username, string password)
        {
            //Proxy is not set for local loopback addresses
            if(String.IsNullOrEmpty(ipAddress) || ipAddress == "127.0.0.1" || ipAddress == "::1")
            {
                return;
            }

            OpenQA.Selenium.Proxy sProxy = new OpenQA.Selenium.Proxy();
            sProxy.Kind = ProxyKind.Manual;
            sProxy.IsAutoDetect = false;
            sProxy.HttpProxy = ipAddress + ":" + port;
            sProxy.SslProxy = ipAddress + ":" + port;
            sProxy.SocksUserName = username;
            sProxy.SocksPassword = username;
            _options.Proxy = sProxy;

            //Workaround - submit login credentials dialog by loading custom extension. Does not work in icognito.
            ProxyExtension proxyExtension = new ProxyExtension();
            proxyExtension.SetConnectionDetails(ipAddress, port, username, password);
            AddExtension(proxyExtension);
        }

        public void SetUserAgent(string userAgent)
        {
            if (!String.IsNullOrEmpty(userAgent))
            {
                _userAgent = $"user-agent={userAgent}";
            }
        }

        public void SetMaximized()
        {
            _options.AddArgument("start-maximized");
        }
    }
}
