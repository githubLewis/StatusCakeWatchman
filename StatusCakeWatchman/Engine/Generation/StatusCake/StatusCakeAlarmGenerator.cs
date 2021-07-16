using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StatusCakeWatchman.Configuration;
using StatusCakeWatchman.Engine.LegacyTracking;
using StatusCakeWatchman.Engine.Logging;

namespace StatusCakeWatchman.Engine.Generation.StatusCake
{
    public class StatusCakeAlarmGenerator : IAlarmGenerator
    {
        private readonly HttpClient _client;
        private readonly IAlarmLogger _logger;
        private int _alarmPutCount;
        private readonly ILegacyAlarmTracker _tracker;
        public StatusCakeAlarmGenerator(HttpClient client, IAlarmLogger logger, ILegacyAlarmTracker tracker)
        {
            _client = client;
            _logger = logger;
            _tracker = tracker;
        }
        public async Task GenerateAlarmsFor(StatusCakeWatchmanConfiguration config, RunMode mode)
        {
            var dryRun = mode == RunMode.DryRun;

            foreach (var alertingGroup in config.Alerts)
            {
                await GenerateAlarmsFor(alertingGroup, dryRun);
            }

            ReportPutCounts(dryRun);
        }

        private void ReportPutCounts(bool dryRun)
        {
            if (dryRun)
            {
                if ((_alarmPutCount > 0))
                {
                    throw new StatusCakeWatchmanException("PUTs happened in dryRun mode");
                }

                _logger.Info("Dry Run: No table or index alarms were put");
                return;
            }

        }

        private async Task GenerateAlarmsFor(StatusCakeAlert alertingGroup, bool dryRun)
        {
            // * Consider dedeuping tests by name (and NOT id)
            var dict = alertingGroup.ToKeyValue();
            
            if (alertingGroup.TestID <= 0)
            {
                dict.Remove("TestID");
            }

            if (!dryRun)
            {
                FormUrlEncodedContent postData = new FormUrlEncodedContent(dict);
                var response = await _client.PutAsync("https://app.statuscake.com/API/Tests/Update?API=opIOwWCieQ5SwSM2uGpv&Username=LewisC", postData);

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStringAsync();
                    var t = JsonConvert.DeserializeObject<StatusCakeResponse>(stream);

                    _logger.Info($"Alarm {t.Data.WebsiteName} has been added.");

                    _alarmPutCount++;
                    _tracker.Register(alertingGroup.WebsiteName);
                }
            }
        }


    }
}