# Create an Azure Active Directory B2C Tenant

# Overview
Before your applications can interact with Azure Active Directory B2C (Azure AD B2C), they must be registered in a tenant that you manage.

### Quick Summary
In this document we will:
- Create an Azure AD B2C tenant
- Link your tenant to your subscription
- Switch to the directory containing your Azure AD B2C tenant
- Add the Azure AD B2C resource as a *Favourite* in the Azure portal.

### Prerequisites
- You must have an existing Azure subscription, before you begin.

### More details
- [Create an Azure AD B2C tenant](#Create-an-Azure-AD-B2C-tenant)
- [Select your B2C tenant directory](#Select-your-B2C-tenant-directory)
- [Add Azure AD B2C as a favorite](#Add-Azure-AD-B2C-as-a-favorite)

### Tasks
The following are suggested tasks, as these may be needed later.
- [ ] Record your Azure AD B2C TenantId
- [ ] Record your Azure AD B2C Initial domain name

#### Create an Azure AD B2C tenant
1. Sign in to the [Azure Portal](https://portal.azure.com/).  Sign in with an Azure account that's been assigned at least the [Contributor](https://docs.microsoft.com/en-us/azure/role-based-access-control/built-in-roles) role within the subscription or a resource group within the subscription.
1. Select the directory that contains your subscription.
	In the Azure portal toolbar, select the *Directory + Subscription* icon, and then select the directory that contains your subscription. This directory is different from the one that will contain your Azure AD B2C tenant.
1. On the Azure portal menu or from the Home page, select *Create a resource*.
1. Search for *Azure Active Directory B2C*, and then select *Create*.
1. Select Create a new *Azure AD B2C Tenant*.
1. On the *Create a directory* page, enter the following:
	- Organization name
	- Initial domain name
	- Country or region
	- Subscription - Select your subscription from the list.
	- Resource group - Select or search for the resource group that will contain the tenant, or select *Create new*.
1. Select *Review + create*.
1. Review your directory settings. Then select Create. For [troubleshooting deployment errors](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/common-deployment-errors).

You can link multiple Azure AD B2C tenants to a single Azure subscription for billing purposes. To link a tenant, you must be an admin in the Azure AD B2C tenant and be assigned at least a Contributor role within the Azure subscription. See [Link an Azure AD B2C tenant to a subscription](https://docs.microsoft.com/en-us/azure/active-directory-b2c/billing#link-an-azure-ad-b2c-tenant-to-a-subscription).

#### Select your B2C tenant directory
To start using your new Azure AD B2C tenant, you need to switch to the directory that contains the tenant.

Select the *Directory + subscription* filter in the top menu of the Azure portal, then select the directory that contains your Azure AD B2C tenant.

If at first you don't see your new Azure B2C tenant in the list, refresh your browser window, then select the *Directory + subscription* filter again in the top menu.

#### Add Azure AD B2C as a favorite
This optional step makes it easier to select your Azure AD B2C tenant in the future.

Instead of searching for Azure AD B2C in All services every time you want to work with your tenant, you can instead favorite the resource. Then, you can select it from the portal menu's Favorites section to quickly browse to your Azure AD B2C tenant.

You only need to perform this operation once. Before performing these steps, make sure you've switched to the directory containing your Azure AD B2C tenant as described in the previous section, [Select your B2C tenant directory](#Select-your-B2C-tenant-directory).

1. Sign in to the [Azure portal](https://portal.azure.com/).
1. In the Azure portal menu, select *All services*.
1. In the All services search box, search for *Azure AD B2C*, hover over the search result, and then select the star icon in the tooltip. *Azure AD B2C* now appears in the Azure portal under *Favorites*.
1. If you want to change the position of your new favorite, go to the Azure portal menu, select *Azure AD B2C*, and then drag it up or down to the desired position.

## More information

- For more information on Azure B2C, see [the Azure AD B2C documentation homepage](http://aka.ms/aadb2c). 
- [Azure portal](https://portal.azure.com/)
- [Tutorial: Create an Azure Active Directory B2C tenant](https://docs.microsoft.com/en-us/azure/active-directory-b2c/tutorial-create-tenant)
- [Link an Azure AD B2C tenant to a subscription](https://docs.microsoft.com/en-us/azure/active-directory-b2c/billing#link-an-azure-ad-b2c-tenant-to-a-subscription)
- [Troubleshooting deployment errors](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/common-deployment-errors)