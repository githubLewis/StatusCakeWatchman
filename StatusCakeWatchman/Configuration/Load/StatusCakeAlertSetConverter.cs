using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatusCakeWatchman.Configuration;

namespace StatusCakeWatchman.Configuration.Load
{
    public class StatusCakeAlertSetConverter : JsonConverter
    {
        private readonly IConfigLoadLogger _logger;

        public StatusCakeAlertSetConverter(IConfigLoadLogger logger)
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

            var result = new StatusCakeAlertSet
            {
                Alerts = jsonObject["Alerts"]?.ToObject<List<StatusCakeAlert>>(serializer) ?? new List<StatusCakeAlert>(),
            };




            return result;
        }

        private static void ReadServiceDefinitions(JObject jsonObject, StatusCakeAlert result, JsonSerializer serializer)
        {
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
