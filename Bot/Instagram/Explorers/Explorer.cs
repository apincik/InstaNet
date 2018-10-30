using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Threading.Tasks;
using Bot.Browser;
using Bot.Exception;
using Bot.Helper;
using System.Globalization;
using System.Collections.Generic;
using Bot.Instagram.Model;
using BaseExplorer = Bot.Browser.Explorer;

namespace Bot.Instagram.Explorers
{
    /// <summary>
    /// Base methods for page elements interaction.
    /// </summary>
    public abstract class Explorer : BaseExplorer
    {
        public Explorer(Config config) : base(config)
        {

        }

        /// <summary>
        /// Random move to element position.
        /// </summary>
        /// <param name="element"></param>
        private void PerformRandomMoveOffset(IWebElement element)
        {
            Actions actions = new Actions(WebDriver);
            Random rand = new Random();
            var x = rand.Next(1, 5); //rand.Next(1, element.Size.Width); //issues with element position
            var y = rand.Next(1, 5); //rand.Next(1, element.Size.Height);
            actions.MoveByOffset(x, y).Perform();
        }

        /// <summary>
        /// Perform move to element and submit click.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isRandomOffsetClick"></param>
        public void PerformMoveAndClick(IWebElement element, bool isRandomOffsetClick = false)
        {
            Actions actions = new Actions(WebDriver);
            if (!isRandomOffsetClick)
            {
                actions.MoveToElement(element).Click().Perform();
            }
            else
            {
                actions.MoveToElement(element);
                Wait(1);
                PerformRandomMoveOffset(element);
                Wait(1);

                actions = new Actions(WebDriver);
                actions.Click().Perform();
            }
        }

        /// <summary>
        /// Perform input fill using IWebDriver Actions
        /// </summary>
        /// <param name="element"></param>
        /// <param name="text"></param>
        /// <param name="isRandomOffsetClick"></param>
        public void PerformInputFill(IWebElement element, string text, bool isRandomOffsetClick = false)
        {
            Actions actions = new Actions(WebDriver);
            if (!isRandomOffsetClick)
            {
                actions.MoveToElement(element).Click().SendKeys(text).SendKeys(OpenQA.Selenium.Keys.Null).Perform();
            }
            else
            {
                actions.MoveToElement(element);
                Wait(1);
                PerformRandomMoveOffset(element);
                Wait(1);

                actions = new Actions(WebDriver);
                actions.Click().SendKeys(text).SendKeys(OpenQA.Selenium.Keys.Null).Perform();
            }
        }

        /// <summary>
        /// Click on bottom navbar profile icon.
        /// </summary>
        public void PerformNavigationToMyProfile()
        {
            try
            {
                var profileNavigation = WebDriver.FindElement(By.XPath(DomSelector.NAVIGATION_PROFILE));
                ExecuteJsClick(profileNavigation);
            }
            catch (NoSuchElementException e)
            {
                throw new RuntimeException("Navigation profile link missing.");
            }
        }

        /// <summary>
        /// Hide browser notifications dialog.
        /// </summary>
        public void HideNotificationsDialog()
        {
            try
            {
                if (WebDriver.FindElements(By.XPath(DomSelector.LAYOUT_DIALOG)).Count > 0)
                {
                    var notificationElement = WebDriver.FindElement(By.XPath(DomSelector.LAYOUT_DIALOG_NOTIFICATIONS_DISMISS));
                    if (notificationElement.Displayed)
                    {
                        notificationElement.Click();
                    }
                }
            }
            catch (NoSuchElementException e)
            {
                return;
            }
        }

        /// <summary>
        /// Dismiss similiar popup dialogs.
        /// </summary>
        public void DismissDialog()
        {
            try
            {
                if (WebDriver.FindElements(By.XPath(DomSelector.LAYOUT_DIALOG)).Count > 0)
                {
                    var buttonElement = WebDriver.FindElement(By.XPath(DomSelector.LAYOUT_DIALOG_BUTTON_DISMISS));
                    if (buttonElement.Displayed)
                    {
                        buttonElement.Click();
                    }
                }
            }
            catch (NoSuchElementException e)
            {
                return;
            }
        }
    }
}
