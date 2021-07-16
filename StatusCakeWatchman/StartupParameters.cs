using CommandLine;
using StatusCakeWatchman.Engine;

namespace StatusCakeWatchman
{
    public class StartupParameters
    {
        [Option("RunMode", Default = RunMode.DryRun,
            HelpText = "RunMode is one of 'TestConfig', 'DryRun' or 'GenerateAlarms'")]
        public RunMode RunMode { get; set; }

        [Option("APIKey", HelpText = "The access key to the AWS account to connect to")]
        public string AwsAccessKey { get; set; }

        [Option("UserName", HelpText = "The secret key to the AWS account to connect to")]
        public string AwsSecretKey { get; set; }

        [Option("ConfigFolder", HelpText = "The location of the config files", Required = true)]
        public string ConfigFolderLocation { get; set; }

        [Option("Verbose", HelpText = "Detailed output", Default = false)]
        public bool Verbose { get; set; }

    }
}
