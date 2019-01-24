using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Bot.Instagram.Explorers;
using Bot.Exception;
using Bot.Instagram.Model;
using Bot.Interface;

namespace Bot.Instagram
{
    /** 
     * Bot logic, handle different actions (opening user profile, like - follow) per method.
     */
    sealed public class Manager : IDisposable
    {
        private readonly string URL_LOGIN = "/accounts/login/";
        private readonly string URL_TAGS = "/explore/tags/";

        private WebExplorer _webExplorer;
        private ActionExplorer _actionExplorer;
        private FeedExplorer _feedExplorer;
        private PhotoExplorer _photoExplorer;
        private UserExplorer _userExplorer;

        private ILogger _logger;
        private Config _config;
        private bool _isLoggedIn = false;

        public Manager(Config config, ILogger logger)
        {
            _webExplorer = new WebExplorer(config);
            _actionExplorer = new ActionExplorer(config);
            _feedExplorer = new FeedExplorer(config);
            _photoExplorer = new PhotoExplorer(config);
            _userExplorer = new UserExplorer(config);
            _logger = logger;
            this._config = config;

            //Onetime browser opening, explorer init.
            _webExplorer.Open();
            _actionExplorer.Open();
            _feedExplorer.Open();
            _photoExplorer.Open();
            _userExplorer.Open();
        }

        /// <summary>
        /// Close window.
        /// </summary>
        public void Dispose()
        {
            _webExplorer.Close();
        }

        /// <summary>
        /// Check current user who is being processed by bot is logged in App and config.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsUserLoggedIn(string username)
        {
            return _isLoggedIn && _config.Username == username;
        }

        /// <summary>
        /// Open user profile from url and follow.
        /// </summary>
        /// <param name="url"></param>
        public void Follow(string url)
        {
            try
            {
                _webExplorer.OpenPage(url);
                _actionExplorer.FollowUser();
            }
            catch(RuntimeException e)
            {
                _logger.LogError(e.Message);
            }
            finally
            {
                _webExplorer.CloseCurrentTab();
            }
        }

        /// <summary>
        /// Open user profile from url and unfollow. @untested
        /// </summary>
        /// <param name="url"></param>
        public void Unfollow(string url)
        {
            try {
                _webExplorer.OpenPage(url);
                _actionExplorer.UnfollowUser();
            }
            catch(RuntimeException e)
            {
                _logger.LogError(e.Message);
            }
            finally
            {
                _webExplorer.CloseCurrentTab();
            }
        }

        /// <summary>
        /// Get user profile stats information.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<Stats> GetProfileStats(string username)
        {
            Stats profileStats = new Stats();

            try
            {
                //driver.OpenUserPageTab(username);
                _webExplorer.PerformNavigationToMyProfile();
                await Task.Delay(Utils.Delay.GetRandomSecondsDelay(2, 5));

                var postsCount = _feedExplorer.GetUserPostsCount();
                var followersCount = _feedExplorer.GetUserFollowersCount();
                var followingCount = _feedExplorer.GetUserFollowingCount();

                //Optionally call Driver.CloseCurrentTab(); when using tabs / urls.
                _webExplorer.GoBack();
                await Task.Delay(Utils.Delay.GetRandomSecondsDelay(1, 5));

                profileStats.PostsCount = postsCount;
                profileStats.FollowingCount = followingCount;
                profileStats.FollowedCount = followersCount;
            }
            catch(RuntimeException e)
            {
                _webExplorer.OpenInstagram("/");
                await Task.Delay(10000);
                _webExplorer.DismissDialog();
                await _logger.LogError(e.Message);
            }

            return profileStats;
        }

        /// <summary>
        /// Run actions over user unfollow list. @unused
        /// </summary>
        /// <param name="unfollowCount"></param>
        /// <param name="randomSkip"></param>
        /// <returns></returns>
        public async Task<int> UnfollowFollowedUsers(int unfollowCount, bool randomSkip)
        {
            try
            {
                _webExplorer.OpenFollowingPageByName(_config.Username);
                return await _actionExplorer.UnfollowFollowedUsers(unfollowCount, randomSkip, true);
            }
            catch(RuntimeException e)
            {
                await _logger.LogError(e.Message);
            }

            return 0;
        }

        /// <summary>
        /// Follow user if profile match behaviour criteria. 
        /// </summary>
        /// <param name="instagramTag"></param>
        /// <param name="followCount"></param>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public async Task<int> FollowFeedUsersByTag(string instagramTag, int followCount = 0, Behaviour behaviour = null)
        {
            Func<Task<bool>> applyFunc = async () =>
            {
                try
                { 
                    var posts = _feedExplorer.GetUserPostsCount();
                    var follows = _feedExplorer.GetUserFollowingCount();
                    var followers = _feedExplorer.GetUserFollowersCount();

                    await Task.Delay(Utils.Delay.GetRandomSecondsDelay(1, 5));

                    //Check behaviour settings before like on post.
                    if (
                        behaviour.MinFollowersToFollowUser > 0 && followers < behaviour.MinFollowersToFollowUser ||
                        behaviour.MaxFollowersToFollowUser > 0 && followers > behaviour.MaxFollowersToFollowUser ||
                        behaviour.MinFollowingToFollowUser > 0 && follows < behaviour.MinFollowingToFollowUser ||
                        behaviour.MaxFollowingToFollowUser > 0 && follows > behaviour.MaxFollowingToFollowUser ||
                        behaviour.MinPostsToFollowUser > 0 && posts < behaviour.MinPostsToFollowUser ||
                        behaviour.MaxPostsToFollowUser > 0 && posts > behaviour.MaxPostsToFollowUser ||
                        !_actionExplorer.FollowUser()
                        )
                    {
                        return false;
                    }
                }
                catch (RuntimeException e)
                {
                    _webExplorer.GoBack();
                    await Task.Delay(1000);
                    _webExplorer.OpenInstagram("/");
                    await Task.Delay(10000);
                    _webExplorer.DismissDialog();
                    await _logger.LogError("FollowFeedUsersByTag runtime exception caught.");
                    return false;
                }

                return true;
            };

            return await ExploreFeedByTag(applyFunc, instagramTag, true, followCount, behaviour);
        }

        /// <summary>
        /// Like user post on page if has lower likes than count from behaviour settings. 
        /// </summary>
        /// <param name="instagramTag"></param>
        /// <param name="likeCount"></param>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public async Task<int> LikeUserPostFromFeedByTag(string instagramTag, int likeCount = 0, Behaviour behaviour = null)
        {
            Func<Task<bool>> applyFunc = async () =>
            {
                try
                {
                    //check behaviour settings before like on post
                    int numOfLikes = _feedExplorer.GetUserPostNumberOfLikes();
                    if (
                        //Error parsing numOflikes from post page.
                        //@todo uncomment to like only photo posts, does now process video views count
                        //numOfLikes == -1 ||

                        //Post has less or more likes then min/max to like.
                        numOfLikes != -1 && behaviour.MinPostLikesToLikePost > 0 && numOfLikes < behaviour.MinPostLikesToLikePost ||
                        numOfLikes != -1 && behaviour.MaxPostLikesToLikePost > 0 && numOfLikes > behaviour.MaxPostLikesToLikePost ||

                        //Error during like occured.
                        !_actionExplorer.LikePostOnPage()    
                        )
                    {
                        return false;
                    }
                } catch(RuntimeException e)
                {
                    _webExplorer.GoBack();
                    await Task.Delay(1000);
                    _webExplorer.OpenInstagram("/");
                    await Task.Delay(10000);
                    _webExplorer.DismissDialog();
                    await _logger.LogError("LikeUserPostFromFeedByTag runtime exception caught.");
                    return false;
                }

                return true;
            };

            return await ExploreFeedByTag(applyFunc, instagramTag, false, likeCount, behaviour);
        }

        /// <summary>
        /// Go over posts in explore feed by provided tag and call provided function to do action.
        /// </summary>
        /// <param name="applyAction"></param>
        /// <param name="instagramTag"></param>
        /// <param name="visitProfile"></param>
        /// <param name="likeCount"></param>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        private async Task<int> ExploreFeedByTag(Func<Task<bool>> applyAction, string instagramTag, bool visitProfile = false, int likeCount = 0, Behaviour behaviour = null)
        {
            int counter = 0;
            Random rand = new Random();
            List<string> visitedLinks = new List<string>();
            bool isAllDone = false;
            //Track count of failed actions, does not match behaviour settings or like action.
            int numberOfFailedActions = 0;  

            try
            {
                //Check for dialog popup
                _webExplorer.DismissDialog();
                try
                {
                    await _webExplorer.PerformExploreTagsPageByTag(instagramTag);
                }
                catch(RuntimeException e)
                {
                    _webExplorer.OpenInstagram($"{URL_TAGS}{instagramTag}/");
                    await Task.Delay(_config.WaitSecondsBetweenActions);
                    _webExplorer.DismissDialog();
                }
                
                await Task.Delay(_config.WaitSecondsBetweenActions);

                if(_webExplorer.IsErrorPage())
                {
                    throw new PageNotFoundException($"Tag page not found {instagramTag}.");
                }
    
                //@TODO Does not solve  further page scrolling in while.
                var pagePostElementsCount = (await _feedExplorer.FindExplorePageElements(false, 1)).Count; 
                
                while (!isAllDone)
                {
                    var postElements = await _feedExplorer.FindExplorePageElements(false, 1);
                    if(postElements.Count == 0)
                    {
                        break;
                    }

                    foreach (var element in postElements)
                    {
                        string href = element.GetAttribute("href");
                        var alreadyVisitedLink = visitedLinks.FirstOrDefault(x => x == href);
                        var randomSkip = rand.Next(0, 5) == 1;

                        //@TOOD also check for visited user profiles, one user can have more photos in feed
                        if (alreadyVisitedLink == null)
                        {
                            visitedLinks.Add(href);
                        }

                        //Prevent early finish before likeCount completed.
                        if (visitedLinks.Count >= pagePostElementsCount)
                        {
                            _webExplorer.ScrollToBottom();
                            await Task.Delay(_config.WaitSecondsBetweenActions);
                            _webExplorer.ScrollToBottom();
                            await Task.Delay(_config.WaitSecondsBetweenActions);
                            var tempOldPageElementsCount = pagePostElementsCount;
                            pagePostElementsCount = (await _feedExplorer.FindExplorePageElements(false, 1)).Count;

                            //If same count of posts event after scroll, finish.
                            if (tempOldPageElementsCount == pagePostElementsCount)
                            {
                                isAllDone = true;
                            }

                            //Break foreach to load new scrolled posts.
                            break;
                        }

                        //If visited, or skip first 9 posts (TOP POSTS) or random skip.
                        if (alreadyVisitedLink != null || visitedLinks.Count <= 9 || randomSkip)
                        {
                            continue;
                        }

                        //Open post page.
                        _webExplorer.PerformMoveAndClick(element);
                        await Task.Delay(_config.WaitSecondsBetweenActions);

                        //Proceed to user profile after clicking on username.
                        if (visitProfile)
                        {
                            var usernameElement = _feedExplorer.GetUsernameElementFromPostPage();
                            _webExplorer.PerformMoveAndClick(usernameElement);
                        }

                        //Custom function call over post or user profile.
                        var result = await applyAction();
                        if (!result)
                        {
                            counter--;
                            numberOfFailedActions++;
                        }

                        if(visitProfile)
                        {
                            await Task.Delay(_config.WaitSecondsBetweenActions);
                            //Go back from user profile to post.
                            _webExplorer.GoBack(); 
                        }

                        await Task.Delay(_config.WaitSecondsBetweenActions);

                        //Go back to feed posts.
                        _webExplorer.GoBack(); 

                        //Wait for next execution.
                        await Task.Delay(_config.WaitSecondsBetweenActions);

                        //Finish when likes done or number of failes is two times more than requested likeCount.
                        if (++counter >= likeCount || numberOfFailedActions > _config.NumberOfFailedActions)
                        {
                            //Will break while loop.
                            isAllDone = true;
                        }

                        //Explicit break @todo replace foreach and random posts pick without loop.
                        break;
                    }
                }

                //Search page.
                _webExplorer.GoBack();

                //Feed page.
                _webExplorer.GoBack();

                //Home feed page.
                _webExplorer.GoBack();

                await Task.Delay(_config.WaitSecondsBetweenActions);
            }
            catch (RuntimeException e)
            {
                _webExplorer.GoBack();
                await Task.Delay(1000);
                _webExplorer.OpenInstagram("/");
                await Task.Delay(10000);
                _webExplorer.DismissDialog();
                await _logger.LogError(e.Message);
                return 0;
            }
            catch(PageNotFoundException e)
            {
                _webExplorer.GoBack();
                await Task.Delay(1000);
                await _logger.LogWarning(e.Message);
                _webExplorer.DismissDialog();
            }

            return counter;
        }

        /// <summary>
        /// Go over explore page posts by tag and like these posts.
        /// @deprecated
        /// </summary>
        /// <param name="instagramTag"></param>
        /// <param name="likeCount"></param>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public async Task<int> LikeFeedPostsByTag(string instagramTag, int likeCount = 0, Behaviour behaviour = null)
        {
            int counter = 0;
            Random rand = new Random();

            try
            {
                _actionExplorer.DismissDialog();
                try
                {
                    await _webExplorer.PerformExploreTagsPageByTag(instagramTag);
                }
                catch (RuntimeException e)
                {
                    _webExplorer.OpenInstagram($"{URL_TAGS}{instagramTag}/");
                    await Task.Delay(_config.WaitSecondsBetweenActions);
                    _webExplorer.DismissDialog();
                }

                if (_webExplorer.IsErrorPage())
                {
                    throw new PageNotFoundException($"LikeFeedPostsByTag tag page not found {instagramTag}.");
                }

                var posts = await _feedExplorer.FindExplorePagePosts(true, 3);
                foreach (var post in posts)
                {
                    _webExplorer.OpenPage(post.url);
                    await Task.Delay(_config.WaitSecondsBetweenActions);

                    //Decrement counter if not liked feed post.
                    if (!_actionExplorer.LikePostOnPage())
                    {
                        counter--;
                    }

                    await Task.Delay(_config.WaitSecondsBetweenActions);
                    _webExplorer.CloseCurrentTab();

                    //Wait for next execution.
                    await Task.Delay(_config.WaitSecondsBetweenActions);

                    if (++counter >= likeCount)
                    {
                        break;
                    }
                }

                _webExplorer.GoBack();
                await Task.Delay(_config.WaitSecondsBetweenActions);
            }
            catch (RuntimeException e)
            {
                await _logger.LogError(e.Message);
            }
            catch(PageNotFoundException e)
            {
                _webExplorer.GoBack();
                await Task.Delay(1000);
                await _logger.LogError(e.Message);
                _webExplorer.DismissDialog();
            }

            return counter;
        }

        /// <summary>
        /// Random feed scroll after login
        /// </summary>
        /// <returns></returns>
        public async Task AfterLogin()
        {
            Random rand = new Random();
            bool randomHomeScroll = _config.RandomHomeScrollAfterLogin && rand.Next(0, 1) == 1;

            //Dismis dialog if any - notifications
            _actionExplorer.DismissDialog();

            if (randomHomeScroll)
            {
                int minScroll = 500;
                int maxScroll = 1500;
                for (int i = 0; i < rand.Next(3, 10); i++)
                {
                    int scroll = rand.Next(minScroll, maxScroll);
                    _webExplorer.Scroll(scroll, 1);
                    minScroll = scroll;
                    maxScroll += scroll;

                    await Task.Delay(Utils.Delay.GetRandomSecondsDelay(1, 3));
                    //Check for notifications dialog
                    _actionExplorer.DismissDialog();
                }
            }

            await Task.Delay(Utils.Delay.GetRandomSecondsDelay(2, 5));
            //Check for notifications dialog
            _actionExplorer.DismissDialog();
        }

        /// <summary>
        /// Run after job run.
        /// </summary>
        public async Task AfterJob()
        {
            _webExplorer.DismissDialog();
        }

        /// <summary>
        /// Handle user login.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Login()
        {
            Random rand = new Random();
            if (_isLoggedIn)
            {
                _webExplorer.OpenInstagram("/");
                return true;
            }

            _webExplorer.OpenInstagram(URL_LOGIN);
            await Task.Delay(rand.Next(1000, 3000));

            try
            {
                await _userExplorer.Login(_config.Username, _config.Password);
                _isLoggedIn = true;
            }
            catch(LoginException e)
            {
                await _logger.LogWarning("Login failed [username=" + _config.Username + "]");
                return false;
            }
            catch(AccountSecurityException e)
            {
                await _logger.LogWarning("Login failed for security reasons [username=" + _config.Username + "]");
                return false;
            }
            catch(RuntimeException e)
            {
                await _logger.LogError(e.Message);
                return false;
            }

            await _logger.LogInfo(String.Format("Login for {0} - success.", _config.Username));
            return true;
        }

        /// <summary>
        /// Call driver to send photo. 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public async Task SubmitPhoto(string path, string desc)
        {
            try
            {
                _webExplorer.DismissDialog();
                await _photoExplorer.SubmitPhoto(path, desc);
            }
            catch (RuntimeException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Vist provided user profile and like post from feed if any found.
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public async Task<JobResult> ActionByUsernames(List<User> users, bool doLike, bool doFollow)
        {
            int processedCount = 0;
            int resultCount = 0;

            try
            {
                foreach (User user in users)
                {
                    //@TODO Possible to use searchbar.
                    _webExplorer.OpenPage(user.Url);
                    var postElements = _feedExplorer.FindUserPagePostsElements();
                    processedCount++;

                    bool userLiked = false;
                    bool userFollowed = false;

                    if (doLike)
                    {
                        if (postElements.Count == 0)
                        {
                            continue;
                        }

                        //Open post page.
                        _webExplorer.ExecuteJsClick(postElements.First());
                        userLiked = _actionExplorer.LikePostOnPage();
                    }
                    
                    userFollowed = doFollow && _actionExplorer.FollowUser();
                    if (userLiked || userFollowed)
                    {
                        resultCount++;
                    }

                    //Go back to homepage feed.
                    _webExplorer.GoBack();
                    await Task.Delay(Utils.Delay.GetRandomSecondsDelay(2, 5));
                    _webExplorer.DismissDialog();
                }
            }
            catch (RuntimeException e)
            {
                await _logger.LogError($"LikeByUsername runtime exception - {e.Message}");
            }

            return new JobResult()
            {
                ProcessedCount = processedCount,
                ResultCount = resultCount
            };
        }

        /// <summary>
        /// Scroll to bottom of the page.
        /// @deprecated
        /// </summary>
        /// <returns></returns>
        public async Task ScrollHomepageFeed()
        {
            for(int i = 1; i <= 10; i++)
            {
                _webExplorer.ScrollToBottom();
                await Task.Delay(10000);
            }
        }
    }
}
