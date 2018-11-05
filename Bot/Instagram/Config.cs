using Bot.Instagram.Model;
using System;

namespace Bot.Instagram
{
    /// <summary>
    /// Bot app config.
    /// </summary>
    //@TODO divide class to Client and Bot config
    public class Config
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool RandomWindowSize { get; set; }

        public Proxy Proxy { get; set; }

        public string UserAgent { get; set; }

        public bool RandomHomeScrollAfterLogin = true;

        //Number of actions when no like/follow is clicked because of behaviour or error during single run.
        private int _numberOfFailedActions = 100;

        public int NumberOfFailedActions
        {
            get
            {
                return _numberOfFailedActions;
            }

            set
            {
                if(value <= 0)
                {
                    throw new ApplicationException("Number of failed actions must be highter than 0.");
                }

                _numberOfFailedActions = value;
            }
        }

        public int MinWaitSecondsBetweenActions = 2;
        public int MaxWaitSecondsBetweenActions = 5;
        public int WaitSecondsAfterJobFinish = 10;

        public int WaitAfterJobFinish
        {
            get
            {
                return WaitSecondsAfterJobFinish * 1000;
            }
        }

        private int? _waitSecondsBetweenActions;
        public int WaitSecondsBetweenActions
        {
            get {
                if (_waitSecondsBetweenActions == null)
                {
                    Random rand = new Random();
                    if(MaxWaitSecondsBetweenActions < MinWaitSecondsBetweenActions)
                    {
                        MaxWaitSecondsBetweenActions = MinWaitSecondsBetweenActions;
                    }

                    return rand.Next(MinWaitSecondsBetweenActions, MaxWaitSecondsBetweenActions) * 1000;
                } else
                {
                    return (int) _waitSecondsBetweenActions * 1000;
                }
            }

            set
            {
                _waitSecondsBetweenActions = value;
            }
        }
    }
}
