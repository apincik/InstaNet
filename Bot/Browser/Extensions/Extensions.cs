using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Browser.Extensions
{
    /// <summary>
    /// Browser extension definition.
    /// </summary>
    abstract class Extensions
    {
        protected static readonly string CURRENT_DIR = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        protected abstract void Init();

        public abstract string Create();
    }
}
