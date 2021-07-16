using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusCakeConfig.Configuration;
using StatusCakeConfig.Engine.Alarms;
using StatusCakeConfig.Engine.Generation;
using StatusCakeConfig.Engine.Logging;

namespace StatusCakeConfig.Engine.LegacyTracking
{
    public interface IOrphanedAlarmReporter
    {
        Task<IReadOnlyList<StatusCakeAlert>> FindOrphanedAlarms();
    }

    public class OrphanedAlarmReporter : IOrphanedAlarmReporter
    {
        // private readonly ILegacyAlarmTracker _tracker;
        private readonly IAlarmFinder _finder;

        public OrphanedAlarmReporter(IAlarmFinder finder, IAlarmLogger logger)
        {
            _finder = finder;
        }

        /*
        public OrphanedAlarmReporter(ILegacyAlarmTracker tracker, IAlarmFinder finder, IAlarmLogger logger)
        {
             _tracker = tracker;
            _finder = finder;
        }
        */

        public async Task<IReadOnlyList<StatusCakeAlert>> FindOrphanedAlarms()
        {
            var allAlarmsBeforeRun = await _finder.AllAlarms();
            // var tracked = _tracker.ActiveAlarmNames;

            var relevant = allAlarmsBeforeRun
                .Where(a => a.WebsiteName != null)
                // all alarms we own should have this, unless they are really really old
                .Where(a =>  a.WebsiteName.IndexOf("StatusCakeConfig",
                                 StringComparison.InvariantCultureIgnoreCase) >= 0)

                .ToList();

            /*
            var unmatched = relevant
                .GroupJoin(tracked, alarm => alarm.AlarmName, name => name,
                    (alarm, enumerable) => (alarm: alarm, owned: enumerable.Any()))
                .Where(x => !x.owned)
                .Select(x => x.alarm)
                .ToArray();
            */

            return Array.Empty<StatusCakeAlert>();
        }
    }
}
