using System.Collections.Generic;

namespace StatusCakeWatchman.Configuration
{
    public class StatusCakeWatchmanConfiguration
    {
        public List<StatusCakeAlert> Alerts { get; set; } = new List<StatusCakeAlert>();
    }
}
