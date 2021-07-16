using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCakeWatchman.Engine
{

    public class StatusCakeWatchmanException : Exception
    {
        public StatusCakeWatchmanException()
        {
        }

        public StatusCakeWatchmanException(string message) : base(message)
        {
        }

        public StatusCakeWatchmanException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
