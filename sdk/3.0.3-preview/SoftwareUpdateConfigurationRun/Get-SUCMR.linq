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

// Get Machine Runs for specific update run
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
string runId;
runId = client.SoftwareUpdateConfigurationRuns.ListByConfigurationName(ResourceGroupName, AutomationAccountName, "nightly run").Value.First().Name;
var runs = client.SoftwareUpdateConfigurationMachineRuns.ListByCorrelationId(ResourceGroupName, AutomationAccountName, Guid.Parse(runId));
Console.WriteLine(runs);

// Get Machine Runs with specific state
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
runs = client.SoftwareUpdateConfigurationMachineRuns.ListByStatus(ResourceGroupName, AutomationAccountName, "Succeeded");
Console.WriteLine(runs);

// Combined filters, parameters are optional and if none passed will return all SUCMRs in account
runs = client.SoftwareUpdateConfigurationMachineRuns.ListAll(ResourceGroupName, AutomationAccountName, 
																null, // can pass Guid.Parse(runId), 
																"Failed",
																"/subscriptions/0d2c19f6-55df-4730-af87-906d19321da2/resourceGroups/myrg/providers/Microsoft.Compute/virtualMachines/myvm"
																);
Console.WriteLine(runs);