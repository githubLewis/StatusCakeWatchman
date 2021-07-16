using Moq;
using StatusCakeWatchman.Engine.Alarms;
using StatusCakeWatchman.Engine.Generation.StatusCake;
using StatusCakeWatchman.Engine.LegacyTracking;
using StatusCakeWatchman.Engine.Logging;
using System.Collections.Generic;
using System.Linq;

namespace StatusCakeWatchman.Tests.Generation.StatusCake.AlarmGeneratorTests
{
    public class StatusCakeAlarmGeneratorMockery
    {
        public StatusCakeAlarmGeneratorMockery()
        {
            AlarmFinder = new Mock<IAlarmFinder>();
            var logger = new ConsoleAlarmLogger(false);

            AlarmGenerator = new StatusCakeAlarmGenerator(new System.Net.Http.HttpClient(), logger, Mock.Of<ILegacyAlarmTracker>());

        }

        public StatusCakeAlarmGenerator AlarmGenerator { get; private set; }

        public Mock<IAlarmFinder> AlarmFinder { get; set; }

    }
}
