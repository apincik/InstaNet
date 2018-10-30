using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Exception
{
    public class LoginException : RuntimeException
    {
        public LoginException(string message) : base(message)    
        {

        }
    }
}
