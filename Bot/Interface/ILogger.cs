using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Interface
{
    /// <summary>
    /// Logger interface definition.
    /// </summary>
    public interface ILogger
    {
        Task LogError(string message);
        Task LogWarning(string message);
        Task LogInfo(string message);

    }
}
