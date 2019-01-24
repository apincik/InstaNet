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
    /// Handling user account actions.
    /// </summary>
    public class UserExplorer : Explorer
    {
        public UserExplorer(Config config) : base(config)
        {

        }

        /// <summary>
        /// Check element if user is logged in.
        /// </summary>
        /// <returns></returns>
        public bool IsLoggedIn()
        {
            IWebElement btn = WaitForElement(5).Until(web =>
            {
                try
                {
                    return WebDriver.FindElement(By.XPath(DomSelector.LAYOUT_LOGGED));
                }
                catch (NoSuchElementException e)
                {
                    return null;
                }
            });

            if (btn == null)
            {
                throw new RuntimeException("IsLoggedIn element exception.");
            }

            return btn.Displayed;
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task Login(string username, string password)
        {
            Random rand = new Random();

            try
            {
                //Insert user credentials
                var usernameInput = WebDriver.FindElementByXPath(DomSelector.LOGIN_INPUT_USERNAME);

                //perform action over input
                PerformInputFill(usernameInput, username, false);
                //usernameInput.SendKeys(username);

                await Task.Delay(rand.Next(2000, 5000));
                //var passwordInput = WebDriver.FindElementByXPath(DomSelector.LOGIN_INPUT_PASSWORD);

                //perform action over input
                //PerformInputFill(passwordInput, password, true);

                SendKeys(OpenQA.Selenium.Keys.Tab);

                //Wait(2);
                await Task.Delay(rand.Next(1000, 2500));

                SendKeys(password);
                //passwordInput.SendKeys(password);
                //Wait(2);
                await Task.Delay(rand.Next(2000, 4000));

                //SendKeys(OpenQA.Selenium.Keys.Enter);

                //submit login
                var submitButton = WebDriver.FindElementByXPath(DomSelector.LOGIN_SUBMIT);
                //PerformMoveAndClick(submitButton, true);

                IJavaScriptExecutor ex = (IJavaScriptExecutor)WebDriver;
                ex.ExecuteScript("arguments[0].click();", submitButton);
                Wait(5);

                await Task.Delay(rand.Next(1000, 2000));

                //text alert
                if (WebDriver.FindElements(By.XPath(DomSelector.LOGIN_ALERT)).Count > 0)
                {
                    throw new LoginException("Bad credentials.");
                }

                //alert popup
                if (WebDriver.FindElements(By.XPath(DomSelector.LOGIN_ALERT_PASSWORD_POPUP)).Count > 0)
                {
                    throw new LoginException("Bad credentials.");
                }

                //click not now if get app dialog is presented
                if (WebDriver.FindElements(By.XPath(DomSelector.LOGIN_UNUSUAL_ATTEMPT_ERROR)).Count > 0)
                {
                    var securityAttemptError = WebDriver.FindElement(By.XPath(DomSelector.LOGIN_UNUSUAL_ATTEMPT_ERROR));
                    if (securityAttemptError.Text == "We Detected An Unusual Login Attempt")
                    {
                        throw new AccountSecurityException("Security code need to be checked.");
                    }
                }

                await Task.Delay(Utils.Delay.GetRandomSecondsDelay(5, 5));

                //click not now if get app dialog is presented
                if (WebDriver.FindElements(By.XPath(DomSelector.LOGIN_GET_APP_DIALOG)).Count > 0)
                {
                    var notNowElement = WebDriver.FindElement(By.XPath(DomSelector.LOGIN_GET_APP_DIALOG));
                    if (notNowElement.Displayed)
                    {
                        notNowElement.Click();
                        Wait(2);
                    }
                }

                await Task.Delay(Utils.Delay.GetRandomSecondsDelay(3, 5));

                //click not now if save password dialog popup
                if (WebDriver.FindElements(By.XPath(DomSelector.LOGIN_SAVE_PASSWORD_DISMISS_BUTTON)).Count > 0)
                {
                    var savePasswordBtnDismiss = WebDriver.FindElement(By.XPath(DomSelector.LOGIN_SAVE_PASSWORD_DISMISS_BUTTON));
                    IJavaScriptExecutor btnSavePasswordDismissEx = (IJavaScriptExecutor)WebDriver;
                    btnSavePasswordDismissEx.ExecuteScript("arguments[0].click();", savePasswordBtnDismiss);
                    Wait(5);
                }

                //Hide desktop notifications dialog if presented
                DismissDialog();

                await Task.Delay(Utils.Delay.GetRandomSecondsDelay(2, 4));

                //Hide add to home screen dialog.
                DismissDialog();

                await Task.Delay(Utils.Delay.GetRandomSecondsDelay(1, 3));

                if (!IsLoggedIn())
                {
                    throw new LoginException("Unknown login fail.");
                }

                /*var test = WebDriver.FindElement(By.XPath("//span[contains(@class, 'glyphsSpriteUser__outline')]"));
                actions = new OpenQA.Selenium.Interactions.Actions(web);
                actions.MoveToElement(test).Click().Perform();
                Wait(10);
                await Task.Delay(rand.Next(5000, 10000));*/

            }
            catch (LoginException e)
            {
                throw e;
            }
            catch (NoSuchElementException e)
            {
                throw new RuntimeException("Finding element during login failed.", e);
            }
            catch (AccountSecurityException e)
            {
                throw e;
            }
        }
    }
}
