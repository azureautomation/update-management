# update-management
Samples for using the update management capabilities in Azure Automation

## Location
SDK Samples can be found under sdk/{version}/samples

## Requirements
These samples uses [LinqPad](https://www.linqpad.net) to provide an easy way to expriment and view the result. In addition to LinqPad, samples use the following nuget packages:
- Microsoft.Azure.Management.Automation (SDK itself)
- Microsoft.Azure.Management.ResourceManager.Fluent (used for authentication)

## Authentication
Service principal authentication is used. To learn about service principal authentication, see [Create Service Principal](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal)

## Configuration file (app.config)
This file contains environment and authentication related information. You need to replace placeholders with your specifc values before you run the samples.

## Samples included

### Software Update Configuration
| Sample                   | Description                                                                        |
| ------------------------ | ---------------------------------------------------------------------------------- |
| Get-SUC                  | Getting Software Update Configuration by name                                      |
| List-SUC                 | Get list of Software Update Configurations in account                              |
| List-SUC-ByVM            | Get list of Software Update Configurations targetting specific VM                  |
| New-SUC-OneTime          | Create new Software Update Configuration on a one time schedule                    |
| New-SUC-Weekly           | Create new Software Update Configuration on a weekly time schedule                 |
| New-SUC-Monthly          | Create new Software Update Configuration on a monthly schedule                     |
| New-SUC-Monthly-TimeZone | Create new Software Update Configuration on a monthly schedule using time zone     |
| New-SUC-Monthly-Adv      | Create new Software Update Configuration with advanced schedule                    |

### Software Update Configuration Run
| Sample          | Description                                                        |
| --------------- | ------------------------------------------------------------------ |
| Get-SUCR        | Get Update Runs with different filters                             |

### Software Update Configuration Machine Run
| Sample          | Description                                                        |
| --------------- | ------------------------------------------------------------------ |
| Get-SUCMR       | Get Update Machine Runs with different filters                     |
