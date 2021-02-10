# ADB2CROPCXamarinForms
This is a simple Xamarin Forms app showcasing how to use ROPC MSAL to authenticate users via Azure Active Directory B2C, and access a Web API with the resulting tokens.

![Sign In](ScreenShots/SignIn.png)
![Signing in](ScreenShots/SigninIn.png)
![Main Page](ScreenShots/MainPage.png)
![Call API](ScreenShots/CallApi.png)
![Sign In - Access denied](ScreenShots/SignIn_AccessDenied.png)
![Sign In - No Password](ScreenShots/SignIn_NoPassword.png)

## Integrate Azure AD B2C into a Xamarin forms app using MSAL and ROPC
This is a simple Xamarin Forms app showcasing how to use MSAL with a ROPC (Resource Owner Password Credentials) flow, to authenticate users via Azure Active Directory B2C,
and access an DOTNET CORE Web API with the resulting token.
- For more information on Azure B2C, see the [Azure AD B2C](https://docs.microsoft.com/en-us/azure/active-directory-b2c/overview) documentation.
- For more information on ROPC flow, see the [Set up a resource owner password credentials flow in Azure Active Directory B2C](https://docs.microsoft.com/en-us/azure/active-directory-b2c/add-ropc-policy).

## How To Run This Sample

To run this sample you will need:
- Visual Studio 2017
- An Internet connection
- An Azure AD B2C tenant
- Exising users in your Azure AD B2C tenant

If you don't have an Azure AD B2C tenant, you can follow [those instructions](https://azure.microsoft.com/documentation/articles/active-directory-b2c-get-started/) to create one. 

*IMPORTANT*: This documentation is not YET complete!
*IMPORTANT*: This documentation is not YET complete!
*IMPORTANT*: This documentation is not YET complete!

### Step 1: Clone or download this repository

From your shell or command line:

```powershell
git clone https://github.com/GeorgeLeithead/ADB2CROPCXamarinForms.git
```

### Step 2: Create your own policies
This sample uses two types of policies: a sign-in policy, and a resource owner password credentials policy (ROPC).
- Create the sign-in policy by following [the instructions here](https://azure.microsoft.com/documentation/articles/active-directory-b2c-reference-policies).  You may choose to include as many or as few identity providers as you wish.
- Create the ROPC pilicy by following [the instructions here](https://docs.microsoft.com/en-us/azure/active-directory-b2c/add-ropc-policy#create-a-resource-owner-user-flow).  You may choose to include as many or as few identity providers as you wish.

*IMPORTANT*: When setting up your identity providers, be sure to [set the redirect URLs](https://docs.microsoft.com/en-us/azure/active-directory-b2c/b2clogin) to use `b2clogin.com`.

If you already have existing policies in your Azure AD B2C tenant, feel free to re-use those.  No need to create new ones just for this sample.

### Step 3: Create your own Navite app
Now you need to [register your native app in your B2C tenant](https://docs.microsoft.com/azure/active-directory-b2c/active-directory-b2c-app-registration#register-a-mobilenative-application), so that it has its own Application ID.

Your native application registration should include the following information:

- Enable the **Native Client** setting for your application.
- Once your app is created, open the app's **Properties** blade and set the **Custom Redirect URI** for your app to `msal<Application Id>://auth`.
- Once your app is created, open the app's **API access** blade and **Add** the API you created in the previous step.
- Copy the Application ID generated for your application, so you can use it in the next step.


### Step 4: Configure the Visual Studio Back-End project with your app coordinates.
This sample includes a sample API.
1. Open the solution in Visual Studio.
1. Open the `BackEnd\ADB2CROPCXamarinForms.HelloService\appsettings.json` file.
1. Find the assignment for `AzureAdB2C:TenantId` and replace XXX with your tenant id.
1. Find the assignment for `AzureAdB2C:Instance` and replace XXX with your Azure AD B2C tenant name.
1. Find the assignment for `AzureAdB2C:ClientId` and replace XXX with the Application ID from step 3.
1. Find the assignment for `AzureAdB2C:Domain` and replace XXX with the Azure AD B2C tenant name.
1. Find the assignment for `AzureAdB2C:SignedOutCallbackPath` and replace XXX the name of the sign-in policy your created in step 2.
1. Find the assignment for `AzureAdB2C:SignUpSignInPolicyId` and replace XXX the name of the sign-in policy your created in step 2.

### Step 5: Configure the Visual Studio mobile application with your app coordinates.
1. Open the solution in Visual Studio.
1. Open the `Mobile\ADB2CROPCXamarinForms' project.
1. Add a new JSON file to the project, called `AppSettings.json`.
1. With the file selected change the properties (Right-click and select 'Properties' OR press F4) "Build Action" to `Embeded Resource`.
1. Edit the `AppSettings.json` file, and add the following content:

```json
{
	// WebApi
	"ApiEndpoint": "localhost",
	"HelloServicePath": "api/Hello",
	"HelloServiceAlivePath": "/Alive",
	// Azure AD B2C coordinates
	"TenantName": "XXX",
	"TenantId": "XXX.onmicrosoft.com",
	"AzureADB2CHostname": "{0}.b2clogin.com",
	"ClientId": "XXX",
	"PolicySignIn": "B2C_1_XXX",
	"PolicyRopc": "B2C_1_XXX",
	"AuthorityBase": "https://{0}/tfp/{1}/",
	"Scopes": "https://{0}/sp/access_as_user",
	// Key Chain group name
	"IosKeychainSecurityGroups": "com.companyname.ADB2CROPCXamarinForms"
}
```
1. Find the assignment for `TenantName` and replace XXX with your Azure AD B2C tenant name.
1. Find the assignment for `TenantId` and replace XXX with your Azure AD B2C tenant name.
1. Find the assignment for `ClientId` and replace XXX with the Application ID from step 3.
1. Find the assignment for `PolicySignIn` and replace XXX the name of the sign-in policy your created in step 2.
1. Find the assignment for `PolicyRopc` and replace XXX the name of the ROPC policy your created in step 2.

### Step 6: Run the BackEnd
From your shell or command line:

```powershell
cd .\ADB2CROPCXamarinForms\src\BackEnd\ADB2CROPCXamarinForms.HelloService
dotnet build
dotnet run
```

### Step 7: Run the mobile app
TODO: Write this so that it's not platform specific!!! Currently I have only tested using the Android emulator!!!

1. Choose the platform you want to work on (Android) by setting the startup project in the Solution Explorer.  Make sure that your platform of choice is marked for build and deploy in the Configuration Manager.
1. Clean the solution, rebuild the solution, and run it.
1. On the Sign-in page, enter the sign-in username and password of a known Azure AD b2C tenant user, and click the sign-in button.  Upon successful sign in, the application screen will list the access token and expires on for the authenticated user and show ba button that allows you to call an API.
1. Close the application and reopen it.  You will see that the app retains access to the API and retrieves the access token and expires on information right away, without the need to sign in again.
1. Sign out by click the `Settings` tab and then the `Sign out` button.

##### Running in an Android Emulator

If you have issues with the Android emulator, please refer to [this document](https://github.com/Azure-Samples/active-directory-general-docs/blob/master/AndroidEmulator.md) for instructions on how to ensure that your emulator supports the features required by MSAL.

### Android specific considerations

The platform specific projects require only a couple of extra lines to accommodate for individual platform differences.

UserDetailsClient.Droid requires one extra line in the `MainActivity.cs` file.
In `OnActivityResult`, we need to add

```csharp
AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);

```
That line ensures that control goes back to MSAL once the interactive portion of the authentication flow ended.

### iOS specific considerations

UserDetailsClient.iOS only requires one extra line, in AppDelegate.cs.
You need to ensure that the OpenUrl handler looks as the snippet below:

```csharp
public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
{
    AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
    return true;
}
```

Once again, this logic is meant to ensure that once the interactive portion of the authentication flow is concluded, the flow goes back to MSAL.

In order to make the token cache work and have the `AcquireTokenSilentAsync` work multiple steps must be followed :

1. Enable Keychain access in your `Entitlements.plist` file and specify in the **Keychain Groups** your bundle identifier.
1. In your project options, on iOS **Bundle Signing view**, select your `Entitlements.plist` file for the Custom Entitlements field.
1. When signing a certificate, make sure XCode uses the same Apple Id. 

## More information

For more information on Azure B2C, see [the Azure AD B2C documentation homepage](http://aka.ms/aadb2c). 