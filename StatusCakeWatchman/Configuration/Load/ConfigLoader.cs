using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using StatusCakeWatchman.Configuration;

namespace StatusCakeWatchman.Configuration.Load
{
    public class ConfigLoader : IConfigLoader
    {
        private readonly FileSettings _fileSettings;
        private readonly IConfigLoadLogger _logger;
        private readonly JsonSerializerSettings _serializationSettings;


        public ConfigLoader(FileSettings fileSettings, IConfigLoadLogger logger)
        {
            _fileSettings = fileSettings;
            _logger = logger;
            _serializationSettings = new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                Converters = new List<JsonConverter>
                {
                    new StatusCakeAlertConverter(_logger),
                    new StatusCakeAlertSetConverter(_logger)
                },
                // ContractResolver = new GreaterThanZeroContractResolver()
            };
        }

        public StatusCakeWatchmanConfiguration LoadConfig()
        {
            var configFolder = _fileSettings.FolderLocation;

            if (string.IsNullOrWhiteSpace(configFolder))
            {
                throw new Exception("Must specify ConfigFolderLocation");
            }

            if (!Directory.Exists(configFolder))
            {
                throw new DirectoryNotFoundException($"Cannot find config folder {configFolder}");
            }

            var configFileNames = Directory.EnumerateFiles(configFolder)
                .Where(fileName => fileName.EndsWith(".json"))
                .ToList();

            if (configFileNames.Count == 0)
            {
                _logger.Error($"No .json files were found in folder {configFolder}");
            }

            var alertingGroups = new List<StatusCakeAlert>();

            foreach (var configFileName in configFileNames)
            {
                var group = LoadGroupFromFile(configFileName);
                if (group != null)
                {
                    if (group.Alerts != null)
                    {
                        if (group.Alerts.Any())
                        {
                            alertingGroups.AddRange(group.Alerts);
                        }
                    }
                }
            }

            _logger.Info($"Read {alertingGroups.Count} alerting groups from {configFileNames.Count} files");

            return new StatusCakeWatchmanConfiguration
            {
                Alerts = alertingGroups
            };
        }

        private StatusCakeAlertSet LoadGroupFromFile(string configFileName)
        {
            try
            {
                var fileContents = File.ReadAllText(configFileName);
                var group = JsonConvert.DeserializeObject<StatusCakeAlertSet>(fileContents, _serializationSettings);

                if (group.Alerts != null)
                {
                    if (group.Alerts.Any())
                    {
                        foreach (var alrt in group.Alerts)
                        {
                            LogStatusCakeAlert(configFileName, alrt);
                        }
                    }
                }
                return group;
            }
            catch (Exception ex)
            {
                throw new ConfigException($"cannot read config file {configFileName}: {ex.Message}", ex);
            }
        }

        private void LogStatusCakeAlert(string configFileName, StatusCakeAlert group)
        {
            
            // _logger.Info($"Read alerting group {group.WebsiteName} containing {containedServiceCounts} from file {configFileName}");

        }

    }
}
