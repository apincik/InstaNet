using System;
using OpenQA.Selenium;
using System.Threading.Tasks;
using Bot.Exception;
using Bot.Helper;
using WebExplorer = Bot.Browser.Explorer;
using AutoIt;

namespace Bot.Instagram.Explorers
{
    /// <summary>
    /// Handling instagram photo post submit.
    /// </summary>
    public sealed class PhotoExplorer : WebExplorer
    {
        public PhotoExplorer(Config config) : base(config)
        {

        }

        /// <summary>
        /// Submit a new photo post.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task SubmitPhoto(string path, string description)
        {
            try
            {
                Random rand = new Random();

                //open feed submit and select photo
                var buttonFeedSubmit = WebDriver.FindElementByXPath(DomSelector.NAVIGATION_POST);
                buttonFeedSubmit.Click();

                await Task.Delay(rand.Next(1000, 3000));
                AutoItX.ControlSend("Open", "", "", path);

                await Task.Delay(rand.Next(1000, 3000));
                AutoItX.ControlSend("Open", "", "", "{ENTER}");

                await Task.Delay(rand.Next(500, 3000));

                //click expand photo
                try
                {
                    var expandButton = WebDriver.FindElementByXPath(DomSelector.CREATE_PHOTO_EXPAND);
                    ExecuteJsClick(expandButton);
                    Wait(5);
                }
                catch(NoSuchElementException e)
                {
                    //ignore expand
                }

                //click next button
                var nextButton = WebDriver.FindElementByXPath(DomSelector.CREATE_PHOTO_BUTTON_NEXT);
                //nextButton.Click();
                ExecuteJsClick(nextButton);
                Wait(5);

                //insert photo description
                if (description != null)
                {
                    var photoDescription = WebDriver.FindElementByXPath(DomSelector.CREATE_PHOTO_DESCSRIPTION);
                    photoDescription.Clear();
                    photoDescription.SendKeys(description);
                    Wait(1);
                }

                await Task.Delay(rand.Next(5000, 12000));

                //share photo
                var shareButton = WebDriver.FindElementByXPath(DomSelector.CREATE_PHOTO_SHARE);
                //shareButton.Click();
                ExecuteJsClick(shareButton);
                Wait(10);
            }
            catch(NoSuchElementException e)
            {
                throw new RuntimeException($"Finding element during submit photo failed - {e.Message}", e);
            }
        }
    }
}
