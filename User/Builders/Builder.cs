using Bot.Instagram.Model;
using System;
using System.Collections.Generic;
using JobEnum = Bot.Instagram.Model.Enum.Job;

namespace User.Builders
{
    /// <summary>
    /// 'Interface' for creating Bot - Job task object.
    /// </summary>
    public abstract class Builder
    {
        private int _id;
        protected int _limit;
        protected string _description;
        protected List<Tag> _tags;
        protected Proxy _proxy;
        protected Settings _settings;
        protected Instagram _instagram;

        protected Job job;

        public Builder()
        {
            _id = 0;
            Init();
        }

        protected abstract int GetJobType();

        private void Init()
        {
            _tags = new List<Tag>();
            _settings = new Settings();
            _proxy = new Proxy();
            _description = "";
            _limit = 0;
        }

        protected virtual void Validate()
        {
            if( _limit > 0 == false ||
                _tags.Count == 0 ||
                _instagram == null)
            {
                throw new Exception("Builder validation error.");
            }
        }

        public virtual Job Build()
        {
            return new Job()
            {
                Id = _id++,
                Limit = _limit,
                LimitDone = 0,
                Description = _description,
                Instagram = _instagram,
                Proxy = _proxy,
                Settings = _settings,
                RunCount = 0,
                Tags = _tags,
                TargetTag = null,
                Type = GetJobType()
            };
        }

        public Builder SetAccount(string username, string password)
        {
            _instagram = new Instagram { Username = username, Password = password };
            return this;
        }

        public Builder SetProxy(string ipAddress, string port, string username, string password)
        {
            _proxy = new Proxy { IpAddress = ipAddress, Port = port, Username = username, Password = password };
            return this;
        }

        public Builder AddTag(string name)
        {
            _tags.Add(new Tag { Name = name });
            return this;
        }

        public Builder SetDescription(string description)
        {
            _description = description;
            return this;
        }

        public Builder SetLimit(int limit)
        {
            _limit = limit;
            return this;
        }
    }
}
