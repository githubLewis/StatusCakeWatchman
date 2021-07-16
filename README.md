# StatusCakeWatchman
Deploy StatusCake tests from the command line, useful for IaC.

### What 

StatusCakeWatchman creates and maintains Alerts in StatusCake.

## Why

Infrastructure as a service, any user with access to repos can easily add alerts.

## Run

`StatusCakeWatchman` is written in C# and .NET Core version 5. [You can download the `dotnet` runtime for Windows, mac or linux here](http://dot.net). It is loosely based on the AWSWatchman software here.

Run the `StatusCakeWatchman` specifying a config folder for config files (see [Configuration file format](ConfigurationFileFormat.md)) and optionally StatusCake credentials.

e.g;


```
dotnet .\StatusCakeWatchman.dll --RunMode GenerateAlarms --ConfigFolder ".\Configs" --UserName XXX --APIKey XXX
```

### Commandline Parameters

The allowed commandline parameters are:

* `RunMode`: One of `TestConfig`, `DryRun`, or `GenerateAlarms`. Optional, default is `DryRun`. Mode behaviours are:
  * `TestConfig`: Configs are loaded and validated. Used to test syntax of changes to config. AWS credentials are not needed.
  * `DryRun`: All actions short of writing alarms are performed. Used to test what the effects of the config will be on the AWS account.
  * `GenerateAlarms`: A full run of the program. You must specify `GenerateAlarms` in order to actually write alarms. 
* `UserName` and `APIKey`. Required. Obtain these credentials from your StatusCake dashbaord. 
* `ConfigFolder`: path to config files. Required.
* `Verbose`: One of `true` or `false`. Give more detailed output. Optional, default is `false`.

#### Test config run

Test that the configs can be read and pass validation. Credentials are not required for this.

e.g.
```
dotnet .\StatusCakeWatchman.dll --RunMode TestConfig --ConfigFolder ".\Configs"
```

#### Dry run

Shows what would happen - It does all the reads but none of the writes. Credentials are required for this.

e.g.
```
dotnet .\Watchman.dll --RunMode DryRun --ConfigFolder ".\Configs" --UserName XXX --APIKey XXX --Verbose true
```
#### Full run

A full read and write run. Credentials are required for this.

e.g.
```
dotnet .\Watchman.dll --RunMode GenerateAlarms --ConfigFolder ".\Configs" --UserName XXX --APIKey XXX
```


## Permissions needed

A user associated with the StatusCake account. Details on obtaining a API key can be found documented here [in the Status Cake Credentials](StatusCakeCredentials.md).

### Run sequence

When run, `StatusCakeWatchman` does things in approximately this order:

- Validate commandline params, and stop if they are invalid.
- Read the config folder and load all alerting groups.
- Validate the alerting groups in the config. Stop if the config is invalid. Stop if the run mode is `TestConfig`.
- Create alarm models
 - Commit the changes. Skipped if the run mode is `DryRun`.
 - Report on "orphans", i.e. resources that are were not created by this tool.

