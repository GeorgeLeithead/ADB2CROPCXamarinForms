[CmdletBinding()]
param(
    [PSCredential] $Credential,

    # Get your Azure tenant ID via URL: https://login.windows.net/YOURDIRECTORYNAME.onmicrosoft.com/.well-known/openid-configuration

    [Parameter(Mandatory = $False, HelpMessage = 'Tenant ID (This is a GUID which represents the "Directory ID" of the AzureAD tenant into which you want to create the apps')]
    [string] $tenantId
)

#Requires -Modules AzureAD

<#
 Before running this script you need to install the AzureAD cmdlets as an administrator.
 For this:
 1) Run Powershell as an administrator
 2) in the PowerShell window, type: Install-Module AzureAD
#>
$ErrorActionPreference = "Stop"

Set-Content -Value "<html><body><h1>Created Users</h1>" -Path ImportedUsers.html

Function Import-TestUsers
{
    <#.Description
   This function imports users into an Azure AD B2C tenant
#>    
    # $tenantId is the Active Directory Tenant. This is a GUID which represents the "Directory ID" of the AzureAD tenant
    # into which you want to create the apps. Look it up in the Azure portal in the "Properties" of the Azure AD.

    # Login to Azure PowerShell (interactive if credentials are not already provided: you'll need to sign-in with creds enabling your to create apps in the tenant)
    if (!$Credential -and $TenantId)
    {
        $creds = Connect-AzureAD -TenantId $tenantId
    }
    else
    {
        if (!$TenantId)
        {
            $creds = Connect-AzureAD -Credential $Credential
        }
        else
        {
            $creds = Connect-AzureAD -TenantId $tenantId -Credential $Credential
        }
    }

    if (!$tenantId)
    {
        $tenantId = $creds.Tenant.Id
    }

    $tenant = Get-AzureADTenantDetail
    $tenantName = ($tenant.VerifiedDomains | Where-Object { $_._Default -eq $True }).Name
    Add-Content -Value "<h2>Tenant</h2>" -Path ImportedUsers.html
    Add-Content -Value "<dl>" -Path ImportedUsers.html
    Add-Content -Value "<dt>Tenant ID:</dt><dd>$tenantId</dd>" -Path ImportedUsers.html
    Add-Content -Value "<dt>Tenant Name:</dt><dd>$tenantName</dd>" -Path ImportedUsers.html
    Add-Content -Value "</dl>" -Path ImportedUsers.html

    $userFromCSV = Import-Csv -Path .\ImportUsers.csv
    Add-Content -Value "<table><thead><tr><th>Display Name</th><th>User Name</th><th>Given Name</th><th>Last Name</th></tr></thead><tbody>" -Path ImportedUsers.html

    foreach($entry in $userFromCSV)
    {
        Add-Content -Value "<tr><td>$($entry.DisplayName)</td><td>$($entry.email)</td><td>$($entry.GivenName)</td><td>$($entry.FamilyName)</td></tr>" -Path ImportedUsers.html
        $PasswordProfile = New-Object -TypeName Microsoft.Open.AzureAD.Model.PasswordProfile
        $PasswordProfile.Password = $entry.password
        $PasswordProfile.ForceChangePasswordNextLogin = $false
        $SignInName = New-Object -TypeName Microsoft.Open.AzureAD.Model.SignInName
        $SignInName.Type = "emailAddress"
        $SignInName.Value = $entry.email
        $NewAduser = New-AzureADUser -DisplayName $entry.DisplayName `
                  -AccountEnabled $true `
                  -SignInNames $SignInName `
                  -GivenName $entry.GivenName `
                  -SurName $entry.FamilyName `
                  -PasswordProfile $PasswordProfile `
                  -CreationType LocalAccount `
                  -JobTitle "Customer"
    }

    Add-Content -Value "</tbody></table></body></html>" -Path ImportedUsers.html
    Write-Host "Complete: See ImportedUsers.html for report."
}

# Pre-requisites
if ($null -eq (Get-Module -ListAvailable -Name "AzureAD")) {
    Install-Module "AzureAD" -Scope CurrentUser
}

Import-Module AzureAD

Import-TestUsers -Credential $Credential -tenantId $TenantId