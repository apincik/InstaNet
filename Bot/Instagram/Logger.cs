using System.Threading.Tasks;
using Bot.Interface;

namespace Bot.Instagram
{
    /// <summary>
    /// Logging interface definition.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Logger<T> : ILogger 
        where T : IClient
    {
        public Logger(T client)
        {

        }

        public abstract Task LogError(string message);
        public abstract Task LogInfo(string message);
        public abstract Task LogWarning(string message);
    }
}
