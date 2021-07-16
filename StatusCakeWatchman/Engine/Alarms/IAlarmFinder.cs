using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using StatusCakeWatchman.Configuration;
using StatusCakeWatchman.Engine.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace StatusCakeWatchman.Engine.Alarms
{
    public interface IAlarmFinder
    {
        Task<StatusCakeAlertsSimple> FindAlarmByName(string alarmName);
        Task<IReadOnlyCollection<StatusCakeAlertsSimple>> AllAlarms();
        int Count { get; }
    }

    public class AlarmFinder : IAlarmFinder
    {

        private readonly IAlarmLogger _logger;
        private readonly HttpClient _alarmClient;

        public AlarmFinder(IAlarmLogger logger, HttpClient alarmClient)
        {
            _logger = logger;
            _alarmClient = alarmClient;
        }

        public int Count => throw new System.NotImplementedException();

        public async Task<IReadOnlyCollection<StatusCakeAlertsSimple>> AllAlarms()
        {
            List<StatusCakeAlertsSimple> existingAlerts = await GetAlerts();

            return existingAlerts;
        }

        private async Task<List<StatusCakeAlertsSimple>> GetAlerts()
        {
            var response = await _alarmClient.GetAsync("https://app.statuscake.com/API/Tests/?API=opIOwWCieQ5SwSM2uGpv&Username=LewisC&WebsiteName=bob");

            var stream = await response.Content.ReadAsStringAsync();

            var existingAlerts = JsonConvert.DeserializeObject<List<StatusCakeAlertsSimple>>(stream);
            return existingAlerts;
        }

        public async Task<StatusCakeAlertsSimple> FindAlarmByName(string alarmName)
        {
            List<StatusCakeAlertsSimple> existingAlerts = await GetAlerts();

            var res= existingAlerts.Where(_ => _.WebsiteName == alarmName).FirstOrDefault<StatusCakeAlertsSimple>();

            return res;
        }
    }
}
