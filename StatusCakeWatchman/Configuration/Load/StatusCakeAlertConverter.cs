using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCakeWatchman.Configuration.Load
{
    class StatusCakeAlertConverter : JsonConverter
    {
        private readonly IConfigLoadLogger _logger;

        public StatusCakeAlertConverter(IConfigLoadLogger logger)
        {
            _logger = logger;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(StatusCakeAlert).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);

            var result = new StatusCakeAlert
            {

                // ViewOnly
                //        int TestID { get; set; }
                // Uptime = (int) jsonObject["Uptime"],
                // NormalisedResponse = (int)jsonObject["NormalisedResponse"],

                TestID = jsonObject["TestID"] != null ? (int)jsonObject["TestID"] : -1,
                WebsiteName = (string)jsonObject["WebsiteName"] ?? string.Empty,
                WebsiteURL = (string)jsonObject["WebsiteURL"] ?? string.Empty,
                CheckRate = jsonObject["CheckRate"] != null ? (int)jsonObject["CheckRate"] : 0,
                TestType = (string)jsonObject["TestType"] ?? string.Empty,
                Paused = (bool)(jsonObject["Paused"] ?? false),
                Port = jsonObject["Port"] != null ? (int)jsonObject["Port"] : -1,
                NodeLocations = (string)jsonObject["NodeLocations"] ?? string.Empty,
                Timeout = jsonObject["Timeout"] != null ? (int)jsonObject["Timeout"] : 30,
                PingURL = (string)jsonObject["PingURL"] ?? string.Empty,
                CustomHeader = (string)jsonObject["CustomHeader"] ?? string.Empty,
                Confirmation = jsonObject["Confirmation"] != null ? (int)jsonObject["Confirmation"] : 0,
                DNSServer = (string)jsonObject["DNSServer"] ?? string.Empty,
                DNSIP = (string)jsonObject["DNSIP"] ?? string.Empty,
                BasicUser = (object)jsonObject["BasicUser"],
                BasicPass = (string)jsonObject["BasicPass"] ?? string.Empty,
                Public = jsonObject["Public"] != null ? (int)jsonObject["Public"] : 0,
                LogoImage = (string)jsonObject["LogoImage"] ?? string.Empty,
                UseJar = jsonObject["UseJar"] != null ? (int)jsonObject["UseJar"] : 0,
                Branding = jsonObject["Branding"] != null ? (int)jsonObject["Branding"] : 0,
                WebsiteHost = (string)jsonObject["WebsiteHost"] ?? string.Empty,
                Virus = jsonObject["Virus"] != null ? (int)jsonObject["Virus"] : 0,
                FindString = (string)jsonObject["FindString"] ?? string.Empty,
                IncludeHeader = jsonObject["IncludeHeader"] != null ? (int)jsonObject["IncludeHeader"] : 0,
                DoNotFind = jsonObject["DoNotFind"] != null ? (int)jsonObject["DoNotFind"] : 0,
                ContactGroup = jsonObject["ContactGroup"]?.ToObject<List<string>>(serializer) ?? new List<string>(),
                RealBrowser = jsonObject["RealBrowser"] != null ? (int)jsonObject["RealBrowser"] : 0,
                TriggerRate = jsonObject["TriggerRate"] != null ? (int)jsonObject["TriggerRate"] : 5,
                TestTags = jsonObject["TestTags"]?.ToObject<List<string>>(serializer) ?? new List<string>(),
                StatusCodes = jsonObject["StatusCodes"]?.ToObject<List<string>>(serializer) ?? new List<string>(),
                EnableSSLAlert = jsonObject["EnableSSLAlert"] != null ? (int)jsonObject["EnableSSLAlert"] : 0,
                FollowRedirect = jsonObject["FollowRedirect"] != null ? (int)jsonObject["FollowRedirect"] : 1,
                PostBody = (string)jsonObject["PostBody"] ?? string.Empty,
                PostRaw = (string)jsonObject["PostRaw"] ?? string.Empty,
                UserAgent = (string)jsonObject["UserAgent"] ?? string.Empty,

            };

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private void ReadIntoTargets(StatusCakeAlert result, JToken jToken)
        {
            if (!jToken.HasValues)
            {
                return;
            }

            foreach (var item in jToken.Children())
            {
                if (item["ContactGroup"] != null)
                {
                    result.ContactGroup.Add(new AlertGroup(item["ContactGroup"].ToString()).ToString());
                }
                else if (item["TestTags"] != null)
                {
                    result.TestTags.Add(new AlertTag(item["TestTags"].ToString()).ToString());
                }
                else
                {
                    _logger.Warn($"The target {jToken} is unknown. Valid targets are 'Email' and 'Url'.");
                }
            }
        }
    }
}
