using Bot.Instagram;
using Bot.Instagram.Model;
using Bot.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Bots
{
    /// <summary>
    /// Base class for custom bot runner objects.
    /// </summary>
    public abstract class Bot
    {
        protected Behaviour _behaviour;
        protected Manager _manager;
        protected IClient _client;
        protected Logger<IClient> _logger;

        public Bot SetBot(Behaviour behaviour, Manager manager, IClient client, Logger<IClient> logger)
        {
            _behaviour = behaviour;
            _manager = manager;
            _client = client;
            _logger = logger;

            return this;
        }

        abstract public Task Run(Job job);

        /**
        * Method performs actions on posts provided by provided function.
        * Expected output of function is count of done actions.
        */
        protected async Task FollowLike(Job job, int atOnce, int perTag, bool split, Func<string, int, Behaviour, Task<int>> followLikeJob)
        {

            Random rand = new Random();

            //Total processed in this run.
            int processedActionsCount = 0;

            //Number of actions to do.
            int toDoActions = job.Limit - job.LimitDone;

            //Do not process more actions  than atOnce value.
            if (toDoActions > atOnce && atOnce > 0)
            {
                toDoActions = atOnce;
            }

            //Split by tags count - same amount of actions.
            List<Tag> currentTags = job.Tags;
            if (split)
            {
                int actionsPerTag = toDoActions / job.Tags.Count;
                //currentTags.ForEach((x) => x.Limit = actionsPerTag);
                int perCount = 0;
                while (perCount < toDoActions)
                {
                    //Iterate tags, randomly set val, if higher than planned toDo count, add 0.
                    currentTags.ForEach((x) =>
                    {
                        if (perCount + actionsPerTag > toDoActions)
                        {
                            actionsPerTag = toDoActions - perCount;
                        }

                        perCount += actionsPerTag;
                        x.Limit = actionsPerTag;
                    });
                }

                //Fullfill all toDoActions, add missing count to first tag.
                if (actionsPerTag * job.Tags.Count < toDoActions)
                {
                    currentTags[0].Limit += toDoActions - (actionsPerTag * job.Tags.Count);
                }

            }
            else
            {
                //Randomly set tags action limit.
                int perCount = 0;
                while (perCount < toDoActions)
                {
                    //Iterate tags, randomly set val, if higher than planned toDo count, add 0.
                    currentTags.ForEach((x) =>
                    {
                        perTag = perTag == 0 ? 1 : perTag;
                        int randActionCount = rand.Next(1, perTag);
                        if (perCount + randActionCount > toDoActions)
                        {
                            randActionCount = toDoActions - perCount;
                        }

                        perCount += randActionCount;
                        x.Limit += randActionCount;
                    });
                }
            }

            //In case of update job process fail or similiar, dont run again
            if (job.LimitDone < job.Limit)
            {
                foreach (Tag tag in currentTags)
                {
                    processedActionsCount += await followLikeJob(tag.Name, tag.Limit, _behaviour);
                }
            }

            if (processedActionsCount > 0)
            {
                await _client.UpdateJobProcess(job.Id, processedActionsCount);
            }

            if (processedActionsCount + job.LimitDone >= job.Limit)
            {
                await _client.SetJobProcessed(job.Id);
            }
        }
    }
}
