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

var ResourceGroupName = ConfigurationManager.AppSettings["ResourceGroupName"];
var AutomationAccountName = ConfigurationManager.AppSettings["AutomationAccountName"];

// Create Software Update Configuration (scheduled deployment)
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Schedule every month at specific time
var scheduleInfo = new ScheduleProperties
{
	Frequency = ScheduleFrequency.Month,
	Interval = 1,
	StartTime = DateTime.Now.AddHours(1).ToUniversalTime(),
};

// Update details
var updateConfiguration = new UpdateConfiguration
{
	OperatingSystem = OperatingSystemType.Windows,
	Windows = new WindowsProperties
	{
		IncludedUpdateClassifications = WindowsUpdateClasses.Critical + ',' + WindowsUpdateClasses.Security,
		ExcludedKbNumbers = new[] { "KB123", "KB123" }
	},

	Duration = TimeSpan.FromHours(3),
	AzureVirtualMachines = new[] {
	   "/subscriptions/0d2c19f6-55df-4730-af87-906d19321da2/resourceGroups/mo-compute/providers/Microsoft.Compute/virtualMachines/mo-vm-w-01",
	   "/subscriptions/0d2c19f6-55df-4730-af87-906d19321da2/resourceGroups/mo-compute/providers/Microsoft.Compute/virtualMachines/mo-vm-w-02"
	}
};

var sucParameters = new SoftwareUpdateConfiguration(updateConfiguration, scheduleInfo);


// Make the call to create the software update configuration
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
var suc = client.SoftwareUpdateConfigurations.Create(ResourceGroupName, AutomationAccountName, $"test-suc-{Guid.NewGuid()}", sucParameters);
Console.WriteLine($"Created '{suc.Name}' in '{suc.ProvisioningState}' state");

// Wait for provisioning to succeed
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
DateTime now = DateTime.Now;
do {
	System.Threading.Thread.Sleep(5000);
	suc = client.SoftwareUpdateConfigurations.GetByName(ResourceGroupName, AutomationAccountName, suc.Name);
	Console.WriteLine(suc.ProvisioningState);
} while (suc.ProvisioningState == "Provisioning" && DateTime.Now - now < TimeSpan.FromMinutes(2));