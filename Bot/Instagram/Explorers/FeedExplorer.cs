using System;
using OpenQA.Selenium;
using System.Threading.Tasks;
using Bot.Exception;
using Bot.Helper;
using System.Globalization;
using System.Collections.Generic;
using Bot.Instagram.Model;

namespace Bot.Instagram.Explorers
{
    /// <summary>
    /// Methods for get results from feed - post pages.
    /// </summary>
    public sealed class FeedExplorer : Explorer
    {
        public FeedExplorer(Config config) : base(config)
        {

        }

        /// <summary>
        /// Return list of posts from explore feed.
        /// </summary>
        /// <param name="scroll"></param>
        /// <param name="minScrollCount"></param>
        /// <returns></returns>
        //@TODO refactor, use result of FindExplorePageElements
        public async Task<List<Post>> FindExplorePagePosts(bool scroll = false, int minScrollCount = 1)
        {
            try
            {
                Random rand = new Random();
                //@TODO refactor wait timeout, without sleep, scroll is not performed
                minScrollCount = rand.Next(minScrollCount, minScrollCount + 10);  //simple random for scrolling overwriting default param
                while (scroll && minScrollCount-- > 0)
                {
                    ScrollToBottom();
                    await Task.Delay(rand.Next(200, 500));
                }

                List<Post> instaPosts = new List<Post>();
                var posts = WebDriver.FindElementsByXPath(DomSelector.EXPLORE_POST);

                foreach (var post in posts)
                {
                    var link = post.FindElement(By.XPath(".//a"));
                    var image = post.FindElement(By.XPath(".//img"));

                    Post instaPost = new Post() { url = link.GetAttribute("href"), imageUrl = image.GetAttribute("src") };
                    instaPosts.Add(instaPost);
                }

                return instaPosts;
            }
            catch (NoSuchElementException e)
            {
                throw new RuntimeException("FindExplorePagePosts element exception.");
            }
        }

        /// <summary>
        /// Return list of elements from explore feed.
        /// </summary>
        /// <param name="scroll"></param>
        /// <param name="minScrollCount"></param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<IWebElement>> FindExplorePageElements(bool scroll = false, int minScrollCount = 1)
        {
            try
            {
                Random rand = new Random();
                //@TODO refactor wait timeout, without sleep, scroll is not performed
                minScrollCount = rand.Next(minScrollCount, minScrollCount + 10);  //simple random for scrolling overwriting default param
                while (scroll && minScrollCount-- > 0)
                {
                    ScrollToBottom();
                    await Task.Delay(rand.Next(200, 500));
                }

                return WebDriver.FindElementsByXPath(DomSelector.EXPLORE_USERPAGE_POST); //EXPLORE_POST
            }
            catch (NoSuchElementException e)
            {
                throw new RuntimeException("FindExplorePagePosts element exception.");
            }
        }

        /// <summary>
        /// Use findElements @deprecated
        /// </summary>
        /// <returns></returns>
        //@TODO refactor possible same as explorePagePosts function
        public List<Post> FindUserPagePosts()
        {
            try
            {
                List<Post> instaPosts = new List<Post>();
                var posts = WebDriver.FindElementsByXPath(DomSelector.EXPLORE_USERPAGE_POST);

                foreach (var post in posts)
                {
                    var link = post.FindElement(By.XPath(".//a"));
                    var image = post.FindElement(By.XPath(".//img"));

                    Post instaPost = new Post() { url = link.GetAttribute("href"), imageUrl = image.GetAttribute("src") };
                    instaPosts.Add(instaPost);
                }

                return instaPosts;
            }
            catch (NoSuchElementException e)
            {
                throw new RuntimeException("FindExplorePagePosts element exception.");
            }
        }

        /// <summary>
        /// Return list of user feed post elements.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyCollection<IWebElement> FindUserPagePostsElements()
        {
            try
            {
                return WebDriver.FindElementsByXPath(DomSelector.EXPLORE_USERPAGE_POST);
            }
            catch (NoSuchElementException e)
            {
                throw new RuntimeException("FindExplorePagePosts element exception.");
            }
        }

        /// <summary>
        /// Get number of post likes.
        /// </summary>
        /// <returns></returns>
        public int GetUserPostNumberOfLikes()
        {
            try
            {
                if (WebDriver.FindElements(By.XPath(DomSelector.POSTPAGE_NUMBER_OF_LIKES)).Count <= 0)
                {
                    return -1;
                }

                int num;
                IWebElement element = WebDriver.FindElementByXPath(DomSelector.POSTPAGE_NUMBER_OF_LIKES);
                return element != null && Int32.TryParse(element.Text, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out num) ? num : -1;
            }
            catch (NoSuchElementException e)
            {
                throw new RuntimeException("FindExplorePagePosts element exception.");
            }
        }

        /// <summary>
        /// Get profile stats number by order in profile.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private float GetProfileStatNumber(int pos)
        {
            try
            {
                var statElements = WebDriver.FindElements(By.XPath(DomSelector.USER_ACCOUNT_STATS));
                if (statElements.Count != 3)
                {
                    throw new NoSuchElementException("Bad count for profile stats.");
                }

                //USER_ACCOUNT_STATS
                var postsCount = statElements[pos];

                var title = postsCount.GetAttribute("title");
                float number = 0;

                if (String.IsNullOrEmpty(title))
                {
                    title = postsCount.Text;
                }

                //if count contains '3.1k' or '3.1m' and no title attribute
                title = title.Replace("k", "00");
                title = title.Replace("m", "00000");

                //title = title.Replace(',', '.');
                float.TryParse(title, NumberStyles.Any, CultureInfo.InvariantCulture, out number);

                return number;

            }
            catch (NoSuchElementException e)
            {
                //return (float) 0;
                throw new RuntimeException("GetProfileStats element exception.");
            }
        }

        public float GetUserPostsCount()
        {
            try
            {
                return GetProfileStatNumber(0);
            }
            catch (RuntimeException e)
            {
                throw e;
            }
        }

        public float GetUserFollowersCount()
        {
            try
            {
                return GetProfileStatNumber(1);
            }
            catch (RuntimeException e)
            {
                throw e;
            }
        }

        public float GetUserFollowingCount()
        {
            try
            {
                return GetProfileStatNumber(2);
            }
            catch (RuntimeException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get username element from photo post page.
        /// </summary>
        /// <returns></returns>
        public IWebElement GetUsernameElementFromPostPage()
        {
            try
            {
                return WebDriver.FindElementByXPath(DomSelector.POSTPAGE_USERNAME);
            }
            catch (NoSuchElementException e)
            {
                throw new RuntimeException("No such element on page - postpage username.");
            }
            catch (StaleElementReferenceException e)
            {
                throw new RuntimeException("No such element on page - postpage username missing title.");
            }
        }

        /// <summary>
        /// Get username from photo post page.
        /// </summary>
        /// <returns></returns>
        public string GetUsernameFromPostPage()
        {
            try
            {
                return GetUsernameElementFromPostPage().GetAttribute("title");
            }
            catch (RuntimeException e)
            {
                throw e;
            }
            catch (StaleElementReferenceException e)
            {
                throw new RuntimeException("No such element on page - postpage username missing title.");
            }
        }
    }
}
