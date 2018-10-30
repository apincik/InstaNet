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
using AutoIt;

namespace Bot.Instagram.Explorers
{
    /// <summary>
    /// Methods for applying actions on instagram pages.
    /// </summary>
    public class ActionExplorer : Explorer
    {
        public ActionExplorer(Config config) : base(config)
        {

        }

        /// <summary>
        /// Unfollow already followed users from my list 
        /// </summary>
        /// <param name="unfollowCount"></param>
        /// <param name="randomSkip"></param>
        /// <param name="scrollToBottom"></param>
        /// <param name="scrollToBottomCount"></param>
        /// <returns></returns>
        public async Task<int> UnfollowFollowedUsers(int unfollowCount, bool randomSkip = false, bool scrollToBottom = false, int scrollToBottomCount = 3)
        {
            Random rand = new Random();
            int counter = 0;

            try
            {
                //@TODO refactor wait timeout, without sleep, scroll is not performed
                //Random for scrolling overwriting default param
                scrollToBottomCount = rand.Next(3, 7);
                while (scrollToBottom && scrollToBottomCount-- > 0)
                {
                    ScrollToBottom();
                    await Task.Delay(rand.Next(200, 500));
                }

                var follows = WebDriver.FindElementsByXPath(DomSelector.UNFOLLOW_FOLLOWED_USERS_LIST);
                foreach (var follow in follows)
                {
                    await Task.Delay(rand.Next(500, 3500));

                    //Skip follower item for behaviour purpose
                    if (randomSkip && rand.Next(0, 2) == 1)
                    {
                        continue;
                    }

                    var unfollowButton = follow.FindElement(By.XPath(".//button"));
                    if (unfollowButton.Text == DomSelector.LABEL_UNFOLLOW_USER)
                    {
                        ExecuteJsClick(unfollowButton);

                        //Wait for popup
                        await Task.Delay(rand.Next(1000, 3000));

                        var unfollowPopupButton = WebDriver.FindElementByXPath(DomSelector.UNFOLLOW_POPUP_BTN);
                        ExecuteJsClick(unfollowPopupButton);
                    }

                    if (++counter >= unfollowCount)
                    {
                        break;
                    }
                }
            }
            catch (NoSuchElementException e)
            {
                throw new RuntimeException("UnfollowFollowedUser element exception.");
            }

            return counter;
        }

        /// <summary>
        /// Post page, including photo, top user, description and others.
        /// </summary>
        /// <returns></returns>
        public bool LikePostOnPage()
        {
            try
            {
                if (WebDriver.FindElements(By.XPath(DomSelector.POSTPAGE_LIKED)).Count > 0)
                {
                    return false;
                }

                var likeButton = WebDriver.FindElementByXPath(DomSelector.POSTPAGE_LIKE_BUTTON);
                ExecuteJsClick(likeButton);
            }
            catch (NoSuchElementException e)
            {
                throw new RuntimeException("LikePostOnPage element exception.");
            }

            return true;
        }

        /// <summary>
        /// Click on follow button on user page.
        /// </summary>
        /// <param name="followText"></param>
        /// <returns></returns>
        private bool ClickOnFollow(string followText)
        {
            try
            {
                var followButton = WebDriver.FindElement(By.XPath(DomSelector.USER_BUTTON_FOLLOW));
                if (followButton.Text != followText)
                {
                    return false;
                }

                ExecuteJsClick(followButton);
            }
            catch (NoSuchElementException e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Submit follow.
        /// </summary>
        /// <returns></returns>
        public bool FollowUser()
        {
            try
            {
                return ClickOnFollow(DomSelector.LABEL_FOLLOW_USER);
            }
            catch (NoSuchElementException e)
            {
                throw new RuntimeException("FollowUser element exception.");
            }
        }

        /// <summary>
        /// Submit unfollow.
        /// </summary>
        /// <returns></returns>
        public bool UnfollowUser()
        {
            try
            {
                return ClickOnFollow(DomSelector.LABEL_UNFOLLOW_USER);
            }
            catch (NoSuchElementException e)
            {
                throw new RuntimeException("UnfollowUser exception.");
            }
        }
    }
}
