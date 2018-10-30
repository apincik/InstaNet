using Bot.Instagram;
using Bot.Instagram.Model;
using User.Builders;
using System;
using System.Threading.Tasks;
using User;
using User.Bots;

namespace Example
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Run(args).GetAwaiter().GetResult();
        }

        static async Task Run(string[] args)
        {
            User.Clients.User client = new User.Clients.User
            {
                onProcessed = (int jobId) => Console.WriteLine($"Job {jobId} processed."),
                onProcessUpdate = (int jobId, int count) => Console.WriteLine($"Job {jobId} updated."),
                onLogMessage = (string message, int type) => Console.WriteLine($"Logged message: {message}"),
                onLogWork = (InstaLog logMessages) =>
                {
                    foreach (var log in logMessages.Data)
                    {
                        Console.WriteLine($"ID: {log.ScheduleId} - logged message: {log.Message}");
                    }
                }
            };

            LikeBuilder builder = new LikeBuilder();
            builder
                //Specify account credentials
                .SetAccount("___USERNAME___", "___PASSWORD___")
                //Add tag (multiple tags allowed)
                .AddTag("instagram")
                //Set likes limit
                .SetLimit(1);

            client.AddJobBuilder(builder);

            Config config = new Config();
            config.UserAgent = "Mozilla/5.0 (Linux; U; Android 4.4.2; en-us; SCH-I535 Build/KOT49H) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";

            Account bot = new Account(client, config);
            bot.Bots.Add(Bot.Instagram.Model.Enum.Job.TYPE_LIKE, new Like());

            await bot.Run();
        }
    }
}
