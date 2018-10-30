using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Utils
{
    /// <summary>
    /// Util for returning miliseconds val.
    /// </summary>
    public class Delay
    {
        private static Random rand;

        private static Random RandomNumber()
        {
            if(rand == null)
            {
                rand = new Random();
            }

            return rand;
        }

        public static int GetRandomSecondsDelay(int min, int max)
        {
            return RandomNumber().Next(min * 1000, max * 1000);
        }

        public static int GetRandomMinutesDelay(int min, int max)
        {
            return RandomNumber().Next(min * 1000 * 60, max * 1000 * 60);
        }

        public static int GetRandomMiliDelay(int min, int max)
        {
            return RandomNumber().Next(min, max);
        }
    }
}
