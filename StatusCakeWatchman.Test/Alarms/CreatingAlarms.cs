using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using StatusCakeWatchman.Configuration;
using StatusCakeWatchman.Engine;
using StatusCakeWatchman.Tests.Generation.StatusCake.AlarmGeneratorTests;

namespace StatusCakeWatchman.Tests.Generation.StatusCake.AlarmGeneratorTests
{
    public class CreatingAlarms
    {
        [Test]
        public async Task AlarmsAreCreated()
        {
            var mockery = new StatusCakeAlarmGeneratorMockery();
            var generator = mockery.AlarmGenerator;

            ConfigureAlarms(mockery);

            await generator.GenerateAlarmsFor(Config(), RunMode.GenerateAlarms);


        }

        private void ConfigureAlarms(StatusCakeAlarmGeneratorMockery mockery)
        {
            // throw new NotImplementedException();
        }

        private static StatusCakeWatchmanConfiguration Config()
        {

            return new StatusCakeWatchmanConfiguration();

        }
    }
}
