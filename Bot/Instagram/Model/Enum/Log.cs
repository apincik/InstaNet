using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Instagram.Model.Enum
{
    public enum Log
    {
        TYPE_NOJOB = 0,
        TYPE_LIKE = 1,
        TYPE_FOLLOW = 2,
        TYPE_UNFOLLOW = 3,
        TYPE_POST = 4,
        TYPE_COMMENT = 5,
        TYPE_STATS = 6,
        TYPE_LOG_LEVEL_INFO = 15,
        TYPE_LOG_LEVEL_WARNING = 16,
        TYPE_LOG_LEVEL_ERROR = 17,
    }
}
