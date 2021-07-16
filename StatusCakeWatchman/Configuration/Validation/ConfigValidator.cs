using System;
using System.Collections.Generic;
using System.Linq;

namespace StatusCakeWatchman.Configuration.Validation
{
    public static class ConfigValidator
    {
        public static void Validate(StatusCakeWatchmanConfiguration config)
        {
            if (config == null)
            {
                throw new ConfigException("Config cannot be null");
            }

            if (!HasAny(config.Alerts))
            {
                throw new ConfigException("Config must have alerting groups");
            }

            foreach (var alertingGroup in config.Alerts)
            {
                Validate(alertingGroup);
            }

            var duplicateNames = config.Alerts
                .Select(g => g.WebsiteName)
                .GroupBy(_ => _)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateNames.Any())
            {
                throw new ConfigException($"The following alerting group names exist in multiple config files: {string.Join(", ", duplicateNames)}");
            }
        }

        private static void Validate(StatusCakeAlert alertingGroup)
        {
            if (string.IsNullOrWhiteSpace(alertingGroup.WebsiteName))
            {
                throw new ConfigException("AlertingGroup must have a name");
            }

            if (string.IsNullOrWhiteSpace(alertingGroup.WebsiteURL))
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' must have a WebsiteURL");
            }

            if (string.IsNullOrWhiteSpace(alertingGroup.TestType))
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' must have a TestType");
            }else if (alertingGroup.TestType != "HTTP" && alertingGroup.TestType != "TCP" && alertingGroup.TestType != "PING" && alertingGroup.TestType != "HEAD" && alertingGroup.TestType != "DNS")
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' must be one of the following types; HTTP,TCP,PING,HEAD,DNS");
            }

            if (!(alertingGroup.CheckRate >= 0 && alertingGroup.CheckRate < 240000))
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' checkrate must be greater than 0 and less than 24000");
            }

            /*
            if (!(alertingGroup.Paused > 1 && alertingGroup.Paused < 0))
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' Paused must be either 1 or 0");
            }
            */

            if (!(alertingGroup.Timeout >= 5 && alertingGroup.Timeout <= 100))
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' Timeout must be greater than 5 and less than 100");
            }

            if (!(alertingGroup.Confirmation >= 0 && alertingGroup.Confirmation <= 4))
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' Confirmation must be greater than 0 and less than 4");
            }

            // TODO: Add IP Check
            if (alertingGroup.DNSIP != "")
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' alertingGroup must be an IP Address FIX ME");
            }

            if (alertingGroup.Public > 1 && alertingGroup.Public < 0)
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' Public must be either 1 or 0");
            }

            if (alertingGroup.UseJar > 1 && alertingGroup.UseJar < 0)
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' UseJar must be either 1 or 0");
            }

            if (alertingGroup.Virus > 1 && alertingGroup.Virus < 0)
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' Virus must be either 1 or 0");
            }

            if (alertingGroup.IncludeHeader > 1 && alertingGroup.IncludeHeader < 0)
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' IncludeHeader must be either 1 or 0");
            }

            if (alertingGroup.RealBrowser > 1 && alertingGroup.RealBrowser < 0)
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' RealBrowser must be either 1 or 0");
            }

            if (alertingGroup.TriggerRate < 0 && alertingGroup.TriggerRate > 60)
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' TriggerRate must be between 0 & 60");
            }

            if (alertingGroup.EnableSSLAlert > 1 && alertingGroup.EnableSSLAlert < 0)
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' EnableSSLAlert must be either 0 or 1");
            }

            if (alertingGroup.FollowRedirect > 1 && alertingGroup.FollowRedirect < 0)
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' FollowRedirect must be either 0 or 1");
            }
        }

        /*
        private static void ValidateTargets(StatusCakeAlert alertingGroup)
        {
            if (alertingGroup.Targets == null)
            {
                throw new ConfigException($"AlertingGroup '{alertingGroup.WebsiteName}' must have targets");
            }

            foreach (var target in alertingGroup.Targets)
            {
                if (target is AlertEmail)
                {
                    var emailTarget = target as AlertEmail;
                    if (string.IsNullOrWhiteSpace(emailTarget.Email))
                    {
                        throw new ConfigException($"Email target for AlertingGroup '{alertingGroup.Name}' must have an email address");
                    }
                }
                else if (target is AlertUrl)
                {
                    var urlTarget = target as AlertUrl;
                    if (string.IsNullOrWhiteSpace(urlTarget.Url))
                    {
                        throw new ConfigException($"Url target for AlertingGroup '{alertingGroup.Name}' must have a url");
                    }

                    try
                    {
                        new Uri(urlTarget.Url);

                    }
                    catch (UriFormatException e)
                    {
                        throw new ConfigException($"Url target '{urlTarget.Url}' for AlertingGroup '{alertingGroup.Name}' is not valid", e);
                    }
                }
                else
                {
                    throw new ConfigException("Unknown target type");
                }
            }
        }
        */

        private static bool HasAny<T>(IEnumerable<T> values)
        {
            return (values != null) && values.Any();
        }

        /// <summary>
        /// "Topic names are limited to 256 characters.
        /// Alphanumeric characters plus hyphens (-) and underscores (_) are allowed."
        /// https://aws.amazon.com/sns/faqs/
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool TextIsValidInSnsTopic(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (value.Length > 100)
            {
                return false;
            }

            return value.All(c => IsAllowedChar(c));
        }

        private static bool IsAllowedChar(char c)
        {
            return char.IsLetterOrDigit(c) || c == '-' || c == '_';
        }
    }
}
