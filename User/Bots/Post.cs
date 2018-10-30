using Bot.Exception;
using Bot.Instagram.Model;
using System.Threading.Tasks;
using ImageUtil = Bot.Utils.Image;

namespace User.Bots
{
    /// <summary>
    /// Bot for submitting post to my profile.
    /// </summary>
    public class Post : Bot
    {
        public override async Task Run(Job job)
        {
            try
            {
                var imageLocalTempPath = job.Image.IsLocal ? job.Image.LocalPath : ImageUtil.DownloadImage(job.Image.Url);
                await _manager.SubmitPhoto(imageLocalTempPath, job.Description);

                //@TODO Remove and set photo job as processed immediately.
                await _client.UpdateJobProcess(job.Id, 1);

                if (job.LimitDone + 1 >= job.Limit)
                {
                    await _client.SetJobProcessed(job.Id);
                }
            }
            catch (RuntimeException e)
            {
                await _logger.LogError($"Photo submit error {e.Message}");
            }
        }
    }
}
