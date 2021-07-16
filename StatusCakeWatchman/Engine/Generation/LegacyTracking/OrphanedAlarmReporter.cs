using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusCakeWatchman.Configuration;
using StatusCakeWatchman.Engine.Alarms;
using StatusCakeWatchman.Engine.Generation;
using StatusCakeWatchman.Engine.Logging;

namespace StatusCakeWatchman.Engine.LegacyTracking
{
    public interface IOrphanedAlarmReporter
    {
        Task<IReadOnlyList<StatusCakeAlertsSimple>> FindOrphanedAlarms();
    }

    public class OrphanedAlarmReporter : IOrphanedAlarmReporter
    {
        private readonly ILegacyAlarmTracker _tracker;
        private readonly IAlarmFinder _finder;
        private readonly IAlarmLogger _logger;

        public OrphanedAlarmReporter(IAlarmFinder finder, IAlarmLogger logger, ILegacyAlarmTracker tracker)
        {
            _finder = finder;
            _tracker = tracker;
            _logger = logger;
        }

        
        public OrphanedAlarmReporter(ILegacyAlarmTracker tracker, IAlarmFinder finder, IAlarmLogger logger)
        {
             _tracker = tracker;
            _finder = finder;
            _logger = logger;
        }
        

        public async Task<IReadOnlyList<StatusCakeAlertsSimple>> FindOrphanedAlarms()
        {
            var allAlarmsBeforeRun = await _finder.AllAlarms();
            var tracked = _tracker.ActiveAlarmNames;

            var relevant = allAlarmsBeforeRun
                .Where(a => a.WebsiteName != null)
                // all alarms we own should have this, unless they are really really old
                .Where(a =>  a.WebsiteName.IndexOf("StatusCakeWatchman",
                                 StringComparison.InvariantCultureIgnoreCase) >= 0)

                .ToList();

            var unmatched = relevant
                .GroupJoin(tracked, alarm => alarm.WebsiteName, name => name,
                    (alarm, enumerable) => (alarm: alarm, owned: enumerable.Any()))
                .Where(x => !x.owned)
                .Select(x => x.alarm)
                .ToArray();

            return unmatched;
        }
    }
}
