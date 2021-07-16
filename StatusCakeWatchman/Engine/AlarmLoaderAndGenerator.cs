using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusCakeWatchman.Configuration;
using StatusCakeWatchman.Configuration.Load;
using StatusCakeWatchman.Configuration.Validation;
using StatusCakeWatchman.Engine.Generation.Generic;
using StatusCakeWatchman.Engine.Generation.StatusCake;
using StatusCakeWatchman.Engine.LegacyTracking;
using StatusCakeWatchman.Engine.Logging;

namespace StatusCakeWatchman.Engine.Generation
{
    public class AlarmLoaderAndGenerator
    {
        private readonly IAlarmLogger _logger;
        private readonly IConfigLoader _configLoader;
        private readonly IAlarmGenerator _statusCakeAlarmGenerator;
        private readonly IOrphanedAlarmReporter _orphanedAlarmReporter;
        private bool _hasRun = false;

        private readonly IAlarmCreator _creator;


        public AlarmLoaderAndGenerator(
            IAlarmLogger logger,
            IConfigLoader configLoader,
            IAlarmGenerator dynamoGenerator,
            IOrphanedAlarmReporter orphanedAlarmReporter,
            IAlarmCreator creator)
        {
            _logger = logger;
            _configLoader = configLoader;
            _statusCakeAlarmGenerator = dynamoGenerator;
            _orphanedAlarmReporter = orphanedAlarmReporter;
            _creator = creator;
        }

        public async Task LoadAndGenerateAlarms(RunMode mode)
        {
            if (_hasRun)
            {
                // there is loads of state etc. and you get duplicate alarms
                // shouldn't happen in real life but I discovered it in tests
                throw new InvalidOperationException($"{nameof(LoadAndGenerateAlarms)} can only be called once");
            }

            try
            {
                _hasRun = true;
                _logger.Info($"Starting {mode}");

                var config = _configLoader.LoadConfig();
                ConfigValidator.Validate(config);

                if (mode == RunMode.GenerateAlarms || mode == RunMode.DryRun)
                {
                    // await GenerateAlarms(config, mode);
                    await _statusCakeAlarmGenerator.GenerateAlarmsFor(config, mode);
                }

                if (mode == RunMode.GenerateAlarms)
                {
                    await LogOrphanedAlarms();
                }

                _logger.Detail("Done");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in run");
                throw;
            }
        }

        private async Task LogOrphanedAlarms()
        {
            _logger.Info("Looking for old alarms");

            var orphans = await _orphanedAlarmReporter.FindOrphanedAlarms();
            _logger.Info(
                $"Found {orphans.Count} alarm(s) that appear to be created by StatusCakeWatchman but are no longer managed:");

            if (orphans.Any())
            {
                foreach (var alarm in orphans)
                {
                    _logger.Info(
                        $" - {alarm.WebsiteName}  "); // (updated: {alarm.AlarmConfigurationUpdatedTimestamp:yyyy-MM-dd})");
                }
            }
        }

    }
}
