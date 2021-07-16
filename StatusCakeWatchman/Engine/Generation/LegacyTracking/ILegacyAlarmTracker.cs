using System.Collections.Generic;

namespace StatusCakeWatchman.Engine.LegacyTracking
{
    public interface ILegacyAlarmTracker
    {
        void Register(string name);
        IReadOnlyCollection<string> ActiveAlarmNames { get; }
    }
}
