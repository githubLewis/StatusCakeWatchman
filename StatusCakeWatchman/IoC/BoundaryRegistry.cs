using System;
using System.Net.Http;
using StatusCakeWatchman.Engine.Logging;
using StructureMap;
using StatusCakeWatchman.Engine;
using StatusCakeWatchman.Configuration;
using StatusCakeWatchman.Configuration.Load;
using StatusCakeWatchman.Engine.LegacyTracking;
using StatusCakeWatchman.Engine.Generation.StatusCake;
using StatusCakeWatchman.Engine.Alarms;
using StatusCakeWatchman.Engine.Generation.Generic;

namespace StatusCakeWatchman.IoC
{
    public class BoundaryRegistry : Registry
    {
        public BoundaryRegistry(StartupParameters parameters)
        {
            For<HttpClient>().Use(_ => new HttpClient()).Singleton();

            SetupLocalDependencies(parameters);
            SetupStatusCakeDependencies(parameters);
        }

        /*
        private TClientConfig CreateClientConfig<TClientConfig>(RegionEndpoint region, HttpClient client)
            where TClientConfig : ClientConfig, new()
        {
            var result = new TClientConfig()
            {
                RegionEndpoint = region,
                MaxErrorRetry = 5,
                Timeout = TimeSpan.FromSeconds(30),
                ReadWriteTimeout = TimeSpan.FromSeconds(90),
                HttpClientFactory = new SingletonHttpClientFactory(client)
            };

            return result;
        }
        */
        private void SetupStatusCakeDependencies(StartupParameters parameters)
        {
            /*
            var region = AwsStartup.ParseRegion(parameters.AwsRegion);
            var creds = AwsStartup.CredentialsWithFallback(
                parameters.AwsAccessKey, parameters.AwsSecretKey, parameters.AwsProfile);

            For<IAmazonDynamoDB>()
                .Use(ctx => new AmazonDynamoDBClient(creds,
                    CreateClientConfig<AmazonDynamoDBConfig>(region, ctx.GetInstance<HttpClient>())))
                .Singleton();
            */


            var fileSettings = new FileSettings(parameters.ConfigFolderLocation);

            For<FileSettings>().Use(fileSettings);

            For<IAlarmCreator>().Use<AlarmCreator>();
            For<IAlarmGenerator>().Use<StatusCakeAlarmGenerator>();
            For<IOrphanedAlarmReporter>().Use<OrphanedAlarmReporter>();

            For<IAlarmFinder>().Use<AlarmFinder>().Singleton();
            For<ILegacyAlarmTracker>().Use<LegacyAlarmTracker>().Singleton();
        }

        private void SetupLocalDependencies(StartupParameters parameters)
        {
            var alarmLogger = new ConsoleAlarmLogger(parameters.Verbose);
            var loadLogger = new ConsoleConfigLoadLogger(parameters.Verbose);

            For<IAlarmLogger>().Use(alarmLogger);
            For<IConfigLoadLogger>().Use(loadLogger);
            For<IConfigLoader>().Use<ConfigLoader>();
            //For<ICurrentTimeProvider>().Use<CurrentTimeProvider>();
        }
    }
}
