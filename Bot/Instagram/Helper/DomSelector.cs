using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Helper
{
    /// <summary>
    /// Instagram DOM selectors for XPath.
    /// </summary>
    public sealed class DomSelector
    {
        public static readonly string CREATE_PHOTO_BUTTON_NEXT = "//button[text()='Next']";

        public static readonly string CREATE_PHOTO_EXPAND = "//span[contains(@class, 'Expand')]";

        public static readonly string CREATE_PHOTO_DESCSRIPTION = "//textarea";

        public static readonly string CREATE_PHOTO_SHARE = "//button[text()='Share']";
        

        public static readonly string LOGIN_BUTTON = "//*[@id='react-root']/section/main/article/div/div/div/span[1]/button";

        public static readonly string LOGIN_SWITCH_FORM = "//*[@id='react-root']/section/main/article/div/div[2]/p/a";

        public static readonly string LOGIN_INPUT_USERNAME = "//*[@name='username']";

        public static readonly string LOGIN_INPUT_PASSWORD = "//*[@name='password']";

        public static readonly string LOGIN_SUBMIT = "//button[text()='Log in']";

        public static readonly string LOGIN_ALERT = "//*[@id='slfErrorAlert']";

        public static readonly string LOGIN_ALERT_PASSWORD_POPUP = "//div[@class='piCib']";

        public static readonly string LOGIN_GET_APP_DIALOG = "//a[text()='Not Now']";

        public static readonly string LOGIN_UNUSUAL_ATTEMPT_ERROR = "//*[@id='react-root']/section/div/div/div[2]/h2";

        public static readonly string LOGIN_SAVE_PASSWORD_DISMISS_BUTTON = "//button[text()='Not Now']";

        public static readonly string LAYOUT_DIALOG = "//div[@role='dialog']";

        public static readonly string ERROR_PAGE_NOT_FOUND = "//div[contains(@class, 'ErrorPage')]";


        //Universal dialog dissmis, click on 2nd button automatically.
        public static readonly string LAYOUT_DIALOG_BUTTON_DISMISS = "//div[@role='dialog']//button[2]";

        public static readonly string LAYOUT_DIALOG_NOTIFICATIONS_DISMISS = "//button[text()='Not Now']";

        public static readonly string LAYOUT_LOGGED = "//span[contains(@class, 'glyphsSpriteNew_post')]"; //checks form submit photo button
        
        public static readonly int LAYOUT_FEED_ITEM_HEIGHT = 700;

        //public static readonly string LAYOUT_DIALOG_ADD_TO_HOME_SCREEN = "//button[text()='Add to Home screen']";

        //public static readonly string LAYOUT_DIALOG_ADD_TO_HOME_SCREEN_DISMISS = "//button[text()='Cancel']";

        //@EXPERIMENTAL
        public static readonly string EXPLORE_SEARCH_RESULT_TAG_FIRST = "//a[contains(@href, 'explore/tags/')][1]";

        //@EXPERIMENTAL
        public static readonly string EXPLORE_SEARCH_INPUT = "//input[@placeholder='Search']";

        //@EXPERIMENTAL
        public static readonly string EXPLORE_SEARCH = "//span[text()='Search']";

        public static readonly string EXPLORE_POST = "//*[contains(concat(' ', normalize-space(@class), ' '), ' kIKUG ')]"; //kIKUG class

        public static readonly string EXPLORE_USERPAGE_POST = "//a[contains(@href, '/p/')]"; //same as explore post



        public static readonly string POSTPAGE_LIKE_BUTTON = "//button[contains(@class, 'coreSpriteHeartOpen')]"; 

        public static readonly string POSTPAGE_UNLIKED = "//span[contains(@class, 'glyphsSpriteHeart__outline')]";

        public static readonly string POSTPAGE_LIKED = "//span[contains(@class, 'glyphsSpriteHeart__filled')]";

        public static readonly string POSTPAGE_USERNAME = "//div[@class='e1e1d']/h2/a";

        public static readonly string POSTPAGE_NUMBER_OF_LIKES = "//a[contains(@href, 'liked_by')]/span";


        public static readonly string USER_ACCOUNT_STATS = "//span[contains(@class, 'g47SY')]";

        public static readonly string USER_BUTTON_FOLLOW = "//button[text()='Follow']";

        public static readonly string USER_FOLLOWING_PAGE = "//a[contains(@href, '/following/')]";

        public static readonly string USER_FOLLOWERS_PAGE = "//a[contains(@href, '/followers/')]";

        public static readonly string POST_LIKED_BY = "//a[contains(@href, '/liked_by/')]";


        public static readonly string UNFOLLOW_FOLLOWED_BTNS = "//button[text()='Following']";

        public static readonly string UNFOLLOW_FOLLOWED_USERS_LIST = "//*[@class='NroHT']";

        public static readonly string UNFOLLOW_POPUP_BTN = "//button[contains(@class, '-Cab_')]";


        public static readonly string LABEL_FOLLOW_USER = "Follow";

        public static readonly string LABEL_UNFOLLOW_USER = "Following";


        public static readonly string NAVIGATION_POST = "//span[contains(@class, 'glyphsSpriteNew_post')]";

        public static readonly string NAVIGATION_PROFILE = "//span[contains(@class, 'glyphsSpriteUser__outline')]";

        public static readonly string NAVIGATION_SEARCH = "//span[contains(@class, 'glyphsSpriteSearch__outline')]";

        public static readonly string NAVIGATION_HOME = "//span[contains(@class, 'glyphsSpriteHome__outline')]/parent::a";
    }
}
