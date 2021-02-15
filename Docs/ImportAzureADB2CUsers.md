# Import Azure Active Directory B2C Users

## Overview
> :warning: We recommend that you run the scripts using option 4, however if your tenant has MFA enabled it is not possible to supply credentials directly, as such use option 3.

### Quick summary

1. On Windows run PowerShell and navigate to the root of the cloned directory
1. In PowerShell run:
   ```PowerShell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force
   ```
1. Run the script to import users.
   ```PowerShell
   cd .\AppCreationScripts\ 
   .\ImportTestUsers.ps1
   ```

### Prerequisites
- You must have an existing Azure Active Directory B2C tenant and tenant id. See [Create an Azure Active Directory B2C tenant](./CreateAzureADB2CTenant.md) for details.
- You must have a valid AD origanisation username and password with rights to manage the Azure Ad B2C tenant.

### More details

- [Present the scripts](#presentation-of-the-scripts) and explain their [usage patterns](#usage-pattern-for-tests-and-devops-scenarios) for test and DevOps scenarios.
- Explain the [pre-requisites](#pre-requisites)
- Explain [four ways of running the scripts](#four-ways-to-run-the-script):
    - [Interactively](#option-1-interactive) to import users to your home tenant
    - [Passing credentials](#option-2-non-interactive) to import users to your home tenant
    - [Interactively in a specific tenant](#option-3-interactive-but-import-users-to-a-specified-tenant)
    - [Passing credentials in a specific tenant](#option-4-non-interactive-and-import-users-to-a-specified-tenant)
- [Dealing with errors](#Dealing-with-errors)

### Tasks
The following are suggested tasks, as these may be needed later.
- [ ] Add users to the `ImportUsers.csv` file
- [ ] Run `ImportUsers.ps1` sctipt.

## Goal of the scripts
The scripts offer a quick and consistent method for creating and configuring (and deleting) Azure AD B2C application registrations.

### Presentation of the scripts

There is one PowerShell scripts and an associated CSV file, which automate the importing of users into an existing Azure AD B2C tenant.

These are:
- `ImportUsers.ps1` which:
    - Reads the ImportUsers.csv file and adds the users and thier details to the Azure Active Directory B2C tenant.
- `ImportUsers.csv` which:
    - This is a CSV file containing import user details.

| DisplayName | GivenName | Familyname | email | password |
| ---- | ---- | ---- | ---- | ---- | ---- |
| This is the users display name, used for chat and easy identification on the Azure Portal. | This is the users given name. | This is the users family name/surname. | This is the users email address *and* sign in user name. | This is the users sign in password. |

### Usage pattern for tests and DevOps scenarios

The `ImportUsers.ps1` will stop if it tries to import an existing user which already exists in the tenant, or the user details are invalid.

## How to use the import script

### Pre-requisites

1. Open PowerShell (On Windows, press  `Windows-R` and type `PowerShell` in the search window)
2. Navigate to the root directory of the project.
3. Until you change it, the default [Execution Policy](https:/go.microsoft.com/fwlink/?LinkID=135170) for scripts is usually `Restricted`. In order to run the PowerShell script you need to set the Execution Policy to `RemoteSigned`. You can set this just for the current PowerShell process by running the command:
    ```PowerShell
    Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process
    ```
### (Optionally) install AzureAD PowerShell modules
The scripts install the required PowerShell module (AzureAD) for the current user if needed. However, if you want to install if for all users on the machine, you can follow the following steps:

4. If you have never done it already, in the PowerShell window, install the AzureAD PowerShell modules. For this:

   1. Open PowerShell as admin (On Windows, Search Powershell in the search bar, right click on it and select Run as administrator).
   2. Type:
      ```PowerShell
      Install-Module AzureAD
      ```

      or if you cannot be administrator on your machine, run:
      ```PowerShell
      Install-Module AzureAD -Scope CurrentUser
      ```
### Run the script

5. Go to the `AppCreationScripts` sub-folder. From the folder where you cloned the repo,
    ```PowerShell
    cd AppCreationScripts
    ```
6. Run the scripts. See below for the [four options](#four-ways-to-run-the-script) to do that.

### Four ways to run the script

We advise four ways of running the script:

- Interactive: you will be prompted for credentials, and the scripts decide in which tenant to create the objects,
- non-interactive: you will provide credentials, and the scripts decide in which tenant to create the objects,
- Interactive in specific tenant:  you will provide the tenant in which you want to create the objects and then you will be prompted for credentials, and the scripts will create the objects,
- non-interactive in specific tenant: you will provide tenant in which you want to create the objects and credentials, and the scripts will create the objects.

Here are the details on how to do this.

#### Option 1 (interactive)

- Just run ``. .\ImportUsers.ps1``, and you will be prompted to sign-in (email address, password, and if needed MFA).
- The script will be run as the signed-in user and will use the tenant in which the user is defined.

Note that the script will choose the tenant in which to create the applications, based on the user. 

#### Option 2 (non-interactive)

When you know the indentity and credentials of the user in the name of whom you want to import the users, you can use the non-interactive approach. It's more adapted to DevOps. Here is an example of script you'd want to run in a PowerShell Window

```PowerShell
$secpasswd = ConvertTo-SecureString "[Password here]" -AsPlainText -Force
$mycreds = New-Object System.Management.Automation.PSCredential ("[login@tenantName here]", $secpasswd)
. .\ImportUsers.ps1 -Credential $mycreds
```

#### Option 3 (Interactive, but import users to a specified tenant)

If you want to import the users to a particular tenant, you can use the following option:
- open the [Azure portal](https://portal.azure.com)
- Select the Azure Active directory you are interested in (in the combo-box below your name on the top right of the browser window)
- Find the "Active Directory" object in this tenant
- Go to **Properties** and copy the content of the *Directory Id* property
- Then use the full syntax to run the scripts:

```PowerShell
$tenantId = "yourTenantIdGuid"
. .\ImportUsers.ps1 -TenantId $tenantId
```
#### Option 4 (non-interactive, and import users to a specified tenant)

This option combines option 2 and option 3: It imports users to a specific tenant. See option 3 for the way to get the tenant Id. Then run:

```PowerShell
$secpasswd = ConvertTo-SecureString "[Password here]" -AsPlainText -Force
$mycreds = New-Object System.Management.Automation.PSCredential ("[login@tenantName here]", $secpasswd)
$tenantId = "yourTenantIdGuid"
. .\ImportUsers.ps1 -Credential $mycreds -TenantId $tenantId
```

### Dealing with errors
- If you recieve an error: `Message: One or more properties contains invalid values.`, then the user attribute *PolicyID* is either not present or the case does not match that required!
- If you receive the following error when using a *non-interactive* option: `AADSTS50076: Due to a configuration change made by your administrator, or because you moved to a new location, you must use multi-factor authentication to access`, then the tenant has MFA enabled and you need to use an *interactive* option.

## More information

- For more information on Azure B2C, see [the Azure AD B2C documentation homepage](http://aka.ms/aadb2c). 
- [Azure portal](https://portal.azure.com/)
- [Overview of user accounts in Azure Active Directory B2C](https://docs.microsoft.com/en-us/azure/active-directory-b2c/user-overview)