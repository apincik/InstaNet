using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Exception
{
    public class AppException : System.Exception
    {
        public AppException(string message) : base(message) {

        }

        public AppException(string message, System.Exception inner) : base(message, inner)
        {

        }

    }
}
