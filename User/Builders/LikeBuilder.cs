using Bot.Instagram.Model;
using JobEnum = Bot.Instagram.Model.Enum.Job;

namespace User.Builders
{
    /// <summary>
    /// Builder for Like job type.
    /// </summary>
    public class LikeBuilder : Builder
    {
        private bool _splitLikesPerAllTags = false;

        protected override int GetJobType()
        {
            return (int) JobEnum.TYPE_LIKE;
        }

        public override Job Build()
        {
            //Run property validation, throwing exception if any value error.
            base.Validate();

            Job job = base.Build();
            job.Settings = new Settings()
            {
                LikesAtOnce = _limit,
                LikesPerTag = 0,
                SplitLikesPerAllTags = _splitLikesPerAllTags ? 1 : 0,
            };

            return job;
        }

        public LikeBuilder SplitLikesPerAllTags(bool value)
        {
            _splitLikesPerAllTags = value;
            return this;
        }
    }
}
