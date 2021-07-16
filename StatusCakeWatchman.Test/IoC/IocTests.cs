using NUnit.Framework;
using StatusCakeWatchman;
using StatusCakeWatchman.Engine.Generation;
using StatusCakeWatchman.Engine.Generation.StatusCake;
using StatusCakeWatchman.IoC;

namespace StatusCakeWatchman.Tests.IoC
{
    [TestFixture]
    public class IocTests
    {
        [Test]
        public void TheAlarmGeneratorResolves()
        {
            var container = new IocBootstrapper()
                .ConfigureContainer(ValidStartupParameters());

            var generator = container.GetInstance<StatusCakeAlarmGenerator>();

            Assert.That(generator, Is.Not.Null);
        }

        [Test]
        public void TheAlarmLoaderAndGeneratorResolves()
        {
            var container = new IocBootstrapper()
                .ConfigureContainer(ValidStartupParameters());

            var loader = container.GetInstance<AlarmLoaderAndGenerator>();

            Assert.That(loader, Is.Not.Null);
        }

        private static StartupParameters ValidStartupParameters()
        {
            return new StartupParameters
            {
                AwsAccessKey = "a",
                AwsSecretKey = "b",
                ConfigFolderLocation = "c:\\temp"
            };
        }

    }
}
