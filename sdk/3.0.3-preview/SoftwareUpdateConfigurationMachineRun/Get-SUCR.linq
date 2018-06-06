<Query Kind="Statements">
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <NuGetReference Prerelease="true">Microsoft.Azure.Management.Automation</NuGetReference>
  <NuGetReference>Microsoft.Azure.Management.ResourceManager.Fluent</NuGetReference>
  <Namespace>Microsoft.Azure.Management.Automation</Namespace>
  <Namespace>Microsoft.Azure.Management.Automation.Models</Namespace>
  <Namespace>Microsoft.Azure.Management.ResourceManager.Fluent</Namespace>
  <Namespace>Microsoft.Azure.Management.ResourceManager.Fluent.Authentication</Namespace>
  <Namespace>System.Configuration</Namespace>
  <AppConfig>
    <Path Relative="..\app.config">&lt;MyDocuments&gt;\LINQPad Queries\UpdateManagement\app.config</Path>
  </AppConfig>
</Query>

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Nugets:
//   Microsoft.Azure.Management.Automation
//   Microsoft.Azure.Management.ResourceManager.Fluent
// Using:
//  System.Configuration
//  Microsoft.Azure.Management.Automation
//  Microsoft.Azure.Management.Automation.Models
//  Microsoft.Azure.Management.ResourceManager.Fluent
//  Microsoft.Azure.Management.ResourceManager.Fluent.Authentication
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

var ResourceGroupName = ConfigurationManager.AppSettings["ResourceGroupName"]; ;
var AutomationAccountName = ConfigurationManager.AppSettings["AutomationAccountName"];

// Use service principal to create Azure Credentials object
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
string tenantId = ConfigurationManager.AppSettings["TenantId"];
var servicePrincipal = new ServicePrincipalLoginInformation
{
	ClientId = ConfigurationManager.AppSettings["ClientId"],
	ClientSecret = ConfigurationManager.AppSettings["ClientSecret"],
};

var credentials = new AzureCredentials(servicePrincipal, tenantId, AzureEnvironment.AzureGlobalCloud);


// Create automation client, and set the resource group and account name context to be used
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
var client = new AutomationClient(credentials)
{
	SubscriptionId = ConfigurationManager.AppSettings["SubscriptionId"]	
};

// Get Runs by SUC name
var runs = client.SoftwareUpdateConfigurationRuns.ListByConfigurationName(ResourceGroupName, AutomationAccountName, "updateDeplymentMultiVm");
Console.WriteLine(runs);

// Get most recent
var mostRecent = runs.Value.FirstOrDefault();
Console.WriteLine(mostRecent);

// Get runs at or after some time
runs = client.SoftwareUpdateConfigurationRuns.ListByStartTime(ResourceGroupName, AutomationAccountName, DateTime.Parse("2018-04-04T07:00:00.0000000Z"));
Console.WriteLine(runs);

// Filter on status
runs = client.SoftwareUpdateConfigurationRuns.ListByStatus(ResourceGroupName, AutomationAccountName, "Succeeded");
Console.WriteLine(runs);

// Filter on OS
runs = client.SoftwareUpdateConfigurationRuns.ListByOsType(ResourceGroupName, AutomationAccountName,"Windows");
Console.WriteLine(runs);

// Combined filters
runs = client.SoftwareUpdateConfigurationRuns.ListAll(ResourceGroupName, AutomationAccountName, "test-suc", "Windows", "failed", DateTime.Parse("2018-04-04T07:00:00.0000000Z").ToUniversalTime());
Console.WriteLine(runs);