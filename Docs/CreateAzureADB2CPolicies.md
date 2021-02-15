# Create Azure AD B2C Policies

# Overview
The sample application makes use of two types of policies: a sign-in policy, and a resource owner password credentials policy (ROPC).  Users are expected to be pre-added to the Azure Acitve Directory B2C tenant, and therefore there is no need for a sign-up policy.

### Prerequisites
- You must have an existing Azure subscription.
- You must have an existing Azure Active Directory B2C tenant.  See [Create an Azure Active Directory B2C tenant](./CreateNewB2CTenant.md) if you dont.

### More details
- [Create a Sign In user flow](#Create-a-Sign-In-user-flow)
- [Create a Sign in using resource owner password credentials](#Create-a-Sign-in-using-resource-owner-password-credentials)

### Tasks
The following are suggested tasks, as these may be needed later.
- [ ] Record your Sign-in policy name
- [ ] Record your ROPC policy name
- [ ] Add user attribute *PolicyId* to Sign-in policy
- [ ] Add user attribute *PolicyId* to ROPC policy

#### Create a Sign In user flow
The sign-in user flow handles sign-in experiences with a single configuration. Users of your application are led down the right path depending on the context.

1. Sign in to the [Azure portal](https://portal.azure.com/).
1. Select the *Directory + Subscription* icon in the portal toolbar, and then select the directory that contains your Azure AD B2C tenant.
1. In the Azure portal, search for and select *Azure AD B2C*.
1. Under Policies, select *User flows*, and then select *New user flow*.
1. On the *Create a user flow page*, select the *Sign in* user flow.
1. Under Select a version, select *Recommended*, and then select *Create*. ([Learn more](https://docs.microsoft.com/en-us/azure/active-directory-b2c/user-flow-versions) about user flow versions.)
1. Enter a *Name* for the user flow - *signin*
1. For *Identity providers*, select *Email signup*.
1. For *User attributes and claims*, select *Show more* and choose as many attributes that will be returned as `claims` as required, and click on *Ok*.
	- Display Name - This is used by the Chat as the users display name and required
1. Click *Create* to add the user flow. A prefix of B2C_1 is automatically prepended to the name.

### Create a Sign in using resource owner password credentials
The Sign in using resource owner password credentials (ROPC) flow enables a user with a local (B2C) account to sign-in directly in native applications (no browser required).

1. Sign in to the [Azure portal](https://portal.azure.com/).
1. Select the *Directory + Subscription* icon in the portal toolbar, and then select the directory that contains your Azure AD B2C tenant.
1. In the Azure portal, search for and select *Azure AD B2C*.
1. Under Policies, select *User flows*, and then select *New user flow*.
1. On the *Create a user flow page*, select the *Sign in using resource owner password credentials (ROPC)* user flow.
1. Under Select a version, select *Preview*, and then select *Create*.
1. Enter a *Name* for the user flow - *ropc*
1. For *User attributes and claims*, select *Show more* and choose as many attributes that will be returned as `claims` as required, and click on *Ok*.
	- Display Name - This is used by the Chat as the users display name and required
1. Click *Create* to add the user flow. A prefix of B2C_1 is automatically prepended to the name.

## More information
- [Creating reference policies](https://azure.microsoft.com/documentation/articles/active-directory-b2c-reference-policies).
- [Creating ROPC policies](https://docs.microsoft.com/en-us/azure/active-directory-b2c/add-ropc-policy#create-a-resource-owner-user-flow).
- [Tutorial: Create user flows in Azure Active Directory B2C](https://docs.microsoft.com/en-us/azure/active-directory-b2c/tutorial-create-user-flows)
- [User flow versions](https://docs.microsoft.com/en-us/azure/active-directory-b2c/user-flow-versions)