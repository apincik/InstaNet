using System;
using System.Threading.Tasks;
using Bot.Instagram.Model;
using Bot.Instagram;
using Bot.Interface;
using Bot.Exception;
using BotLog = Bot.Instagram.Model.Log;
using LogEnum = Bot.Instagram.Model.Enum.Log;
using User.Bots;
using System.Collections.Generic;

namespace User
{
    /// <summary>
    /// Handle init of Bot Manager, running jobs and closing active web session.
    /// </summary>
    /// <remarks>
    /// Supported bots for like, follow, post, stats.
    /// </remarks>
    public class Account
    {
        private Behaviour _behaviour;
        private Manager _manager;
        private IClient _client;
        private Logger<IClient> _logger;
        private Config _config;

        public Factory Bots;
        public bool IsRunning = true;

        public Account(IClient client)
        {
            Bots = new Factory();
            _behaviour = new Behaviour();
            //Random refresh values for Behaviour object, could be overwritten in SetupManager
            _behaviour.RandomRefresh();  

            _client = client;
            _logger = new User.Log.Logger(client);
            _manager = null;
        }

        public Account(IClient client, Config config) : this(client)
        {
            _config = config;
        }

        public async Task Run()
        {
            Random rand = new Random();
            Schedule schedule = new Schedule();
            List<string> failedUsernameLogin = new List<string>();

            try
            {
                //Get jobs list from API.
                schedule = await _client.FindJobs();

                foreach (Job job in schedule.Jobs)
                {
                    //Setup new manager if job account changes or does not exist.
                    SetupManager(job);

                    //Check for previously failed login attempt.
                    if (failedUsernameLogin.Exists(x => x == _config.Username))
                    {
                        continue;
                    }
                    //Login to acc if not logged in already from previous job.
                    if (!(await _manager.Login()))
                    {
                        failedUsernameLogin.Add(_config.Username);
                        continue;
                    }

                    await _manager.AfterLogin();

                    //@TODO !DELETE!
                    //job.Type = (int) Bot.Instagram.Model.Enum.Job.TYPE_SCRAPE;
                    //job.TargetTag = "refreshersk";

                    //Perform bot action
                    Bots.Get(job.Type)
                        .SetBot(_behaviour, _manager, _client, _logger)
                        .Run(job)
                        .Wait();

                    await _manager.AfterJob();
                    await Task.Delay(_config.WaitAfterJobFinish);
                }
            }
            catch (ApplicationException e)
            {
                await _client.LogMessage($"Application error - {e.Message}", (int) LogEnum.TYPE_LOG_LEVEL_ERROR);
            }
            finally
            {
                /** Finish, close window. */
                if (_manager != null)
                {
                    _manager.Dispose();
                    _manager = null;
                }
            }
        }

        /**
         * Set manager object by job data for account. 
         */
        protected void SetupManager(Job job)
        {
            if (_manager == null || !_manager.IsUserLoggedIn(job.Instagram.Username))
            {
                if (_manager != null)
                {
                    _manager.Dispose();
                    _manager = null;
                }

                Config accountConfig = _config == null ? new Config() : _config;
                accountConfig.Username = job.Instagram.Username;
                accountConfig.Password = job.Instagram.Password;
                accountConfig.RandomWindowSize = true;
                accountConfig.Proxy = job.Proxy;

                //Set new account, update behaviour by current job data.
                _config = accountConfig;
                _manager = new Manager(accountConfig, (ILogger) _logger);
                _behaviour.SetValues(job.Settings);
            }
        }
    }
}
