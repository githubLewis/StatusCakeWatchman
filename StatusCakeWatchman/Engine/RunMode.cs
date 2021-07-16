using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCakeWatchman.Engine
{
    public enum RunMode
    {
        None = 0,
        TestConfig,
        DryRun,
        GenerateAlarms
    }
}
