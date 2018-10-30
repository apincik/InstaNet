## InstaNet
Instagram automation bot project, supported extensions and behaviour edit.

## Disclaimer
Research purpose as similiar projects available on GitHub. No responsibility for results of any kind of use of this repository code.

## Issues & requests
Submit any request or issue to project issues tracker.

### Features

- Likes (*feed*)
- Follows (*feed*)
- Submit photo post
- Profile stats
- Unfollow (**not used**)

### Requirements
- running using Selenium (ChromeDriver), tested on Windows platform, targeting .NET 4.6.x.
- in order to use proxy extension, include content of *include* folder into your executable location

### Featured interfaces
- available LikeBuilder to create Bot job object

### Contribution
- Feel free to fork repository, test your code and submit pull request.

### Custom Extensions
- see wiki page (https://github.com/apincik/InstaNet/wiki/) how to write custom extensions

### Common issues
- check project solution in order to resolve packages/references

### Feature updates
- [ ] namespace updates
- [ ] application config split
- [ ] HTML DOM selectors move to separete loader
- [ ] new bot builders
- [ ] UI for application management
- [ ] new bot scenarios (*issues tracking*)

*With BC break prevention*

### Example
- see Example project inside repository

```c#
User.Clients.User client = new User.Clients.User
{
    onProcessed = (int jobId) => Console.WriteLine($"Job {jobId} processed.")
};

LikeBuilder builder = new LikeBuilder();
builder.SetAccount("___USERNAME___", "___PASSWORD___").AddTag("instagram").SetLimit(1);
client.AddJobBuilder(builder);

Config config = new Config();
config.UserAgent = "Mozilla/5.0 (Linux; U; Android 4.4.2; en-us; SCH-I535 Build/KOT49H) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";
config.MinWaitSecondsBetweenActions = 5;
config.MaxWaitSecondsBetweenActions = 15;

Account bot = new Account(client, config);
bot.Bots.Add(Bot.Instagram.Model.Enum.Job.TYPE_LIKE, new Like());

await bot.Run();
```
