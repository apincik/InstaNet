using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Exception
{
    public class PageNotFoundException : System.Exception
    {
        public PageNotFoundException(string message) : base(message) {

        }

        public PageNotFoundException(string message, System.Exception inner) : base(message, inner)
        {

        }

    }
}
