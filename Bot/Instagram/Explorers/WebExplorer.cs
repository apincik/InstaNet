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

namespace Bot.Instagram.Explorers
{
    /// <summary>
    /// Interaction with browser page loading.
    /// </summary>
    public class WebExplorer : Explorer
    {
        private readonly string URL_INSTAGRAM_DOMAIN = "https://www.instagram.com";

        public WebExplorer(Config config) : base(config)
        {

        }

        /// <summary>
        /// Open page on instagram.com domain.
        /// </summary>
        /// <param name="page"></param>
        public void OpenInstagram(string page = "")
        {
            WebDriver.Url = URL_INSTAGRAM_DOMAIN + page;
            WebDriver.Navigate();
            Wait(5);
        }

        /// <summary>
        /// Open page by username.
        /// </summary>
        /// <param name="username"></param>
        public void OpenUserPageByName(string username)
        {
            string url = $"{URL_INSTAGRAM_DOMAIN}/{username}/";
            OpenPage(url);
        }

        /// <summary>
        /// Open following page by username.
        /// </summary>
        /// <param name="username"></param>
        public void OpenFollowingPageByName(string username)
        {
            try
            {
                string url = $"{URL_INSTAGRAM_DOMAIN}/{username}/following/";
                WebDriver.Url = url;
                WebDriver.Navigate();
                Wait(3);

                //FIX - current link navigate only to profile page
                var followingPageLink = WebDriver.FindElement(By.XPath(DomSelector.USER_FOLLOWING_PAGE));
                ExecuteJsClick(followingPageLink);
                Wait(5);
            }
            catch (NoSuchElementException e)
            {
                throw new RuntimeException("OpenFollowingPageByName element exception.");
            }
        }

        /// <summary>
        /// Open explore tags page by tag name.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task OpenExploreTagsPageByTag(string tag)
        {
            try
            {
                var searchEl = WebDriver.FindElement(By.XPath(DomSelector.NAVIGATION_SEARCH));
                PerformMoveAndClick(searchEl, false);
                await Task.Delay(Utils.Delay.GetRandomMiliDelay(100, 1000));

                var inputEl = WebDriver.FindElement(By.XPath(DomSelector.EXPLORE_SEARCH));
                PerformMoveAndClick(inputEl, false);
                await Task.Delay(Utils.Delay.GetRandomMiliDelay(100, 500));

                //send hashtag string
                SendKeys("#" + tag);
                await Task.Delay(Utils.Delay.GetRandomMiliDelay(1000, 3000));

                //pick first tag (@TODO In some cases, desired tag do not need to be as first result!)
                var tagElement = WebDriver.FindElement(By.XPath(DomSelector.EXPLORE_SEARCH_RESULT_TAG_FIRST));
                ExecuteJsClick(tagElement);
            }
            catch (NoSuchElementException e)
            {
                throw new RuntimeException("Open Explore page missing element.");
            }
        }
    }
}
