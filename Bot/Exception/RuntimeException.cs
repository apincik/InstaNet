using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Exception
{
    public class RuntimeException : System.Exception
    {
        public RuntimeException(string message) : base(message) {

        }

        public RuntimeException(string message, System.Exception inner) : base(message, inner)
        {

        }

    }
}
