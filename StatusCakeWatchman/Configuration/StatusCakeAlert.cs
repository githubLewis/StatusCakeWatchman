using System.Collections.Generic;

namespace StatusCakeWatchman.Configuration
{
    public class StatusCakeAlertsSimple
    {
        [GreaterThanZeroContractResolver]
        public int TestID { get; set; }
        public bool Paused { get; set; }
        public string TestType { get; set; }
        public string WebsiteName { get; set; }
        public string WebsiteURL { get; set; }
        public int CheckRate { get; set; }
        public List<string> ContactGroup { get; set; }
        public int ContactId { get; set; }
        public string Status { get; set; }
        public int Uptime { get; set; }
        public List<string> Tags { get; set; }
        public string WebsiteHost { get; set; }
    }

    public class StatusCakeAlert : StatusCakeAlertsSimple
    {
        /* All now defined on StatusCakeAlertsSimple
        // Required
        //public string WebsiteName { get; set; }
        //public string WebsiteURL { get; set; }
        //public int CheckRate { get; set; }
        //public string TestType { get; set; }

        // Optional
        //public int TestID { get; set; }
        //public bool Paused { get; set; }
        //public string WebsiteHost { get; set; }
        //public List<string> ContactGroup { get; set; }
        */

        public int Port { get; set; }
        public string NodeLocations { get; set; }
        public int Timeout { get; set; } = 30;
        public string PingURL { get; set; }
        public string CustomHeader { get; set; }
        public int Confirmation { get; set; } = 0;
        public string DNSServer { get; set; }
        public string DNSIP { get; set; }
        public object BasicUser { get; set; }
        public string BasicPass { get; set; }
        public int Public { get; set; } = 0;
        public string LogoImage { get; set; }
        public int UseJar { get; set; } = 0;
        public int Branding { get; set; } = 0;
        public int Virus { get; set; }
        public string FindString { get; set; }
        public int IncludeHeader { get; set; }
        public int DoNotFind { get; set; }
        public int RealBrowser { get; set; }
        public int TriggerRate { get; set; } = 5;
        public List<string> TestTags { get; set; }
        public List<string> StatusCodes { get; set; }
        public int EnableSSLAlert { get; set; } = 0;
        public int FollowRedirect { get; set; } = 1;
        public string PostBody { get; set; }
        public string PostRaw { get; set; }
        public string UserAgent { get; set; }
    }

}
